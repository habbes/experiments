# Experiment proposals

This is a list of experiments that I think we should explore and evaluate to help reach a conclusion or recommendation about whether and how we should integrate `Utf8JsonWriter`, what the potential gains would be and what tread-offs should be considered.

The experiments are listed in ascending order of the perceived difficulty in implementing. The potential performance gains are also expected to increase. We should assess both the performance again as well the amount of effort it would take to implement that proposal as well as any thing we'd have to sacrifice (e.g. features/flexibility, etc.). Some proposals may require breaking changes to the public API.

The end goal is to considerally improve OData writer's performance (CPU usage, memory, latency, throughput) without sacrificing the correctness of the results with respect to OData specifications (e.g. structure of the payload, annotations, validation with respect to the model where applicable, etc.).

Contents:
- [0. Evaluation](#0-evaluation)
- [1. Wrap the synchronous writer in a `BufferedStream`](#1-wrap-the-synchronous-writer-in-a-bufferedstream)
- [2. Provide a custom `IJsonWriter` using `Utf8JsonWriter` via depdency injection](#2-provide-a-custom-ijsonwriter-using-utf8jsonwriter-via-depdency-injection)
- [3. Using a shared `IBufferWriter<byte>` for both `Utf8JsonWriter` and `Stream`/`TextWriter` APIs](#3-using-a-shared-ibufferwriterbyte-for-both-utf8jsonwriter-and-streamtextwriter-apis)
- [4. Moonshot: Generate strongly-typed converters from model to avoid boxing](#4-moonshot-generate-strongly-typed-converters-from-model-to-avoid-boxing)
- [5. Moonshot: Generate strongly-typed model-based serializers at compile time](#5-moonshot-generate-strongly-type-model-based-serializers-at-compile-time)

## 0. Evaluation

To evaluate the performance gains from these experiments, we can use the performance tests in `OData.net` repo. However, those tests are not suitable for evaluating async writers because they don't simulate concurrent requests. In the existing test, async writers perform worse than the sync versions since they just measure serializing a single payload at a time. Furthermore, the service tests that simulate sending requests to an actual server are broken. We should add tests to fill these gaps in order to properly evaluate performance in realistic scenarios:
- Tests that measure latency and throughput where requests are handled concurrently
- Tests that simulate requests between a client and server

## 1. Wrap the synchronous writer in a `BufferedStream`

The `ODataJsonLightOutputContext` wraps the output `Stream` in an `AsyncBufferedStream` to provide some buffering when in async mode. But in synchronous mode, the stream is not buffered. In a [recent PR](https://github.com/OData/odata.net/pull/2222) the custom `AsyncBufferedStream` class was replaced with the standard library's new `BufferedStream` which resulted in a peformance increase. It is not clear to me why in the synchronous mode the `BufferedStream` is not buffered.

One quick thing to try would be to wrap the sync `Stream` in a `BufferedStream` and check whether that improves perf. This is a low-effort experiment that does not require any breaking changes.

I am also curious to know what the performance for the async scenario would be without the `BufferedStream`.

## 2. Provide a custom `IJsonWriter` using `Utf8JsonWriter` via depdency injection

The OData writer allows injecting a custom `IJsonWriter` implementation. To do this, we have to create a class that implements `IJsonWriterFactory` then inject that in the service container. To provide dependency injection capabilities the `IODataRequestMessage`/`IODataResponseMessage` should also implement `IContainerProvider`. This means it should expose an `Container` property which is the actual `IServiceProvider`.

`IJsonWriter` and `Utf8JsonWriter` operate at similar level of granularity (writing properties and primitive values, starting/ending object and arrays, etc.) so implementing `IJsonWriter` using `Utf8JsonWriter` should be possible. However, there are couple of complications:
- `IJsonWriter` is expected to write to a `TextWriter` as opposed to a stream directly (the `IJsonWriterFactory.CreateJsonWriter` expects a `TextWriter` as paramter). `Utf8JsonWriter` either takes a `Stream` or an `IBufferWriter` as input.
- OData provides both `IJsonWriter` and `IJsonWriterAsync` APIs. These declare sync and async versions of the same methods. The default `JsonWriter` implements both interfaces and is used for writing both synchronously and asynchronously. `Utf8JsonWriter` only has synchronous write methods. It writes synchronously to the `IBufferWriter`'s memory and only sends the data to the stream when you `Flush()` or `FlushAsync()`.
- `Utf8JsonWriter` outputs the JSON to the stream as UTF8 bytes. `IJsonWriter` writes to a `TextWriter` passed by the caller and the encoding is set by the caller.
- `JsonWriter` also implements `IJsonStreamWriter` which expose methods that return a `Stream` or `TextWriter` to the caller, allowing the call to write output directly to those streams. This is used when writing binary data in a streaming fashion, e.g. piping from one stream to another without reading all the bytes from the input stream into memory in advance. The stream is wrapped in an `ODataBinaryStreamWriter` whichs does a bit of buffering, the result is eventually converted to to base64 before being written to the final destination by `JsonWriter` (TODO: confirm this)
- `IJsonWriter` supports JSONP and exposes `WritePaddingFunctionName` and `Start/StopFunctionPaddingScope()` for writing the JSONP wrapper function. `Utf8JsonWriter` does not provide JSONP support.

If we create a custom `IJsonWriter` implementation, we'd have to workaround those constraints and make them fit even if it's not the most optimal way of using a `Utf8JsonWriter`, the benefit would be that integration with OData writer would be "plug-and-play" and not require any breaking changes. The tight coupling between `IJsonWriter` and `TextWriter` complicates a potential integration unreasonably. For this reason I will first propose an implementation that does not require `TextWriter` then propose how we could workaround those to make things work with `TextWriter`. While removing the `TextWriter` coupling would be a breaking change, I still think this proposal is worthwhile to look at first because:
- The `TextWriter` coupling should be reconsidered whether or not we decide to integrate `Utf8JsonWriter`. I strongly propose that we remove this requirement in OData Core 8.x, it makes the `IJsonWriter` less flexible.
- It would be relatively easy to replace the `TextWriter` parameter in `IJsonWriterFactory` with `Stream`. I don't think there are many (if any) users of OData who actually provide a custom `IJsonWriterFactory` anyway. And if there are, they could pass the underlying the stream of the `TextWriter` directly instead.
- For the `StartTextWriterScope()` method which returns a `TextWriter`, we could change the implementation to wrap the TextWriter around the stream we now have instead of the `TextWriter`

For the illustrative purposes, let's call the proposed custom implementation `Utf8ODataJsonWriter`. `Utf8ODataJsonWriter`'s constructor will take a `Stream` as input and create a new `Utf8JsonWriter` instance around it. We implement the `IJsonWriter.WriteValue(***)` methods by calling the corresponding `Utf8JsonWriter.Write***Value()` (e.g.: `WriteValue(int value)` => `WriteIntValue(int value))`. This will write the data to the buffer's memory until the `Utf8ODataJsonWriter` is flushed. We can set a threshold of how much data should be buffered before writing to the stream and check that method after every write operation. So each `WriteValue()` method will check the `Utf8ODataJsonWriter.BytesPending` against a threshold and call `Utf8ODataJsonWriter.Flush` if the threshold is reached. The async versions can do the same thing but call `Utf8ODataJsonWriter.FlushAsync()`.

Here's a summary of what the `IJsonWriter` -> `Utf8JsonWriter` mapping could look like:

IJsonWriter | Utf8JsonWriter 
------------|---------------
`WriteName(string name)` | `WritePropertyName(string name)`. *See notes on string escaping below*.
`WriteValue(bool value)` | `WriteBooleanValue(bool value)`
`Writevalue(int value)` | `WriteNumberValue(int value)`. *See notes on number formatting below*.
`WriteValue(short value)` | `WriteNumberValue(int value)` (`short` not supported, so we can cast to int)
`WriteValue(byte value)` | `WriteNumberValue(int value)` (`byte` not supported, so we can cast to int)
`WriteValue(sbyte value)` | `WriteNumberValue(int value)` (`sbyte` not supported, so we can cast to int)
`WriteValue(float value)` | if `value` is +/-infiniy or NaN, then write `"INF"`, `"-INF"` or `"NaN"`, otherwise `WriteNumberValue(float value)`. *See notes on number formatting below*.
`WriteValue(double value)` | if `value` is +/-infiniy or NaN, then write `"INF"`, `"-INF"` or `"NaN"`, otherwise `WriteNumberValue(double value)`. *See notes on number formatting below*.
`WriteValue(long value)` | if `isIeee75Compatible` then convert to a string and call `WriteStringValue(value.ToString(InvariantCulture))` otherwise call `WriteNumberValue(long value)`. *See notes on number formatting below*.
`WriteValue(long value)` | if `isIeee75Compatible` then convert to a string and call `WriteStringValue(value.ToString(InvariantCulture))` otherwise call `WriteNumberValue(long value)`. *See notes on number formatting below*.
`WriteValue(decimal value)` | if `isIeee75Compatible` then convert to a string and call `WriteStringValue(value.ToString(InvariantCulture))` otherwise call `WriteNumberValue(decimal value)`. *See notes on number formatting below*.
`WriteValue(Guid value)` | `WriteStringValue(Guid value)`
`WriteValue(DateTimeOffset)` | `WriteStringValue(DateTimeoffset)`. *See notes on dates and times below*.
`WriteValue(TimeSpan)` | *See notes on dates and times below*.
`WriteValue(Date)` | *See notes on dates and times below*
`WriteValue(TimeOfDay)` | *See notes on dates and times blow*
`WriteValue(string value)` | if value is null `WriteNullValue()`, otherwise `WriteStringValue(string value)`. *See notes on string escaping below*.
`WriteValue(byte[] value)` | This writes the `byte[]` array as base64-encoded string. We can use the OData routine to convert the `byte[]` array into chunks of base64-encoded `chart[]` arrays, then write those chunks using `WriteStringValue(ReadOnlySpan<char>)`
`WriteRawValue(string)` | `WriteRawValue(string)`
`StartArrayScope` | `WriteStartArray()`
`EndArrayScope` | `WriteEndArray()`
`StartObjectScope` | `WriteStartObject()`
`EndObjectScope` | `WriteEndObject()`
`WriteFunctionPaddingName` | *see notes on JSONP support below*
`StartFunctionPaddingScope` | *see notes on JSONP support below*
`EndFunctionPaddingScope` | *see notes on JSONP support below*
`StartStreamValueScope` | *See notes on providing Stream for directing binary writing below*
`EndStreamValueScope` | *See notes on providing Stream for directing binary writing below*
`StartTextWriterValueScope(string contentType)` | *See notes on Providing TextWriter for directing writing below*
`EndTextWriterValueScope()` |  *See notes on Providing TextWriter for directing writing below*

### Notes on recursion depth

`Utf8JsonWriter` has a default max recursion depth of 64. This means that writing JSON with more than 64 levels of nested will throw an exception. `ODataMessageWriterSettings` sets a default max recursion depth of 100. This means writing could potentially fail due to recursion depth errors. We should set the max depth of `Utf8JsonWriter` to at least that of `ODataMessageWriter`.

### Notes on string escaping

`JsonWriter` escapes property names and all string values written with `WriteValue(string value)` and `WriteName(string name)`. Values written with `WriteRawValue(string value)` or most string values from conversions of other primitive types. However, when in `Ieee754Compatible` mode, where `long`s and `double`s are quoted, they are also escaped.

By default, OData uses the `ODataStringEscapeOption.EscapeNonAscii` which escapes all control characters (like `\n`) and non-ascii characters. `Uf8JsonWriter` allows you to specify a `TextEncoder` which will handle the escaping. When none is provided, it will use a default `JavaScriptEncoder` which also escapes non-ascii chars(outside basic-latin Unicode range). So we can rely on `Utf8JsonWriter`'s escaping instead of using our own (much less efficient) implementation. `Utf8JsonWriter`'s default `JavaScriptEncoder` also escapes sensitive HTML characters like `<` and `>` to protect against XSS attacks if the payload is rendered in an HTML page. I'm not sure if OData escapes such characters, if it doesn't, we could consider the `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` which doesn't escape HTML-sensitive characters, or find an encoder which produces the same results as OData.

### Notes on number formatting

When `JsonWriter` writes numerical values, it first converts them to string using `value.ToString(CultureInfo.InvariantCulture)`. `Utf8JsonWriter` uses `Utf8Formatter.TryFormat` to try and convert the values. I'm not sure if the two are always equivalent, especially for decimals. We should verify that the two formatting mechanisms produce the same results, otherwise we can convert to string as done in `JsonWriter` and write that string using `WriteRawValue()`.

If `IEEE754Compatible` is enabled, longs, and decimals are wrapped in quotes like strings. For `floats` and `doubles`, `value.ToString()` has a max scale that's smaller than `float/double.MaxValue`, so `JsonWriter` uses `XmlConvert.ToString()` instead of `value.ToString()`.

### Notes on dates and times

`Utf8JsonWriter` provides methods to write `DateTimeOffset` and `DateTime` but doesn't have overloads for `DateTime`, `TimeOfDay` and `Date`. `Utf8JsonWriter` writes date times as [ISO 8601-1 extended format](https://docs.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support) (`YYYY-MM-DDThh:mm:ss[.s]TZD where TZD = Z or +-hh:mm.`). `JsonWriter` does the same (it uses `XmlConvert.ToString`). For the types which are not covered by `Utf8JsonWriter`, we can convert to string the same way OData does it and write the raw string values.

### Notes on JSONP support

`Utf8JsonWriter` does not support JSONP. JSONP wraps a JSON object in a function call (e.g.: `paddingFunction({ "key": "value" }))`). The resulting payload is technically not valid JSON (due to function wrapping). To provide support for `JSONP`, the JSONP related methods (`WriteFunctionPaddingName`, `StartFunctionPaddingScope`, `EndFunctionPaddingScope`) can write directly to the stream instead of `Utf8JsonWriter`. When writing to the stream directly, we should ensure that we write utf-8 encoded bytes.

I would propose the we re-evaluate whether we should continue to support JSONP in the next major release, given that this is an outdated technology with known security issues and has largely been superseded by CORS.

### Notes on providing a stream for direct binary writing

`JsonWriter` implements a `StartStreamValueScope` which returns a `Stream`. This allows the caller to write bytes directly to the stream, and then call `EndStreamValueScope` once done. `JsonWriter` encloses the data written to the stream within quotes. The data is actually treated as a binary string. The stream returned by `JsonWriter` is an `ODataBinaryWriterStream`. This class wraps the output `TextWriter` and overrides the `Stream.Write` methods to convert the bytes to base64-encoded string before writing them to the `TextWriter`.

This [test](https://github.com/OData/odata.net/blob/6643a9ef12388b07a46f3b460576e215ceb4d9b8/test/FunctionalTests/Microsoft.OData.Core.Tests/JsonLight/ODataJsonLightOutputContextApiTests.cs#L285) demonstrate how this feature can be applied to write binary content for the value of a stream property.

To implement this in `Utf8ODataJsonWriter` would be a bit tricky since `Utf8JsonWriter` does not provide a way to write a JSON value one chunk at a time, and it doesn't expose methods to write to the stream directly. A naive approach would be to return a memory stream, then once the `EndStreamValueScope` is called, we convert the bytes to a base64 string and call `WriteRawValue()` in one chunk. This would be problematic if the data to write is too large or inefficient to keep in memory all at once.

Another approach is to write the chunks directly to the underlying stream, bypassing `Utf8JsonWriter`. But if `Utf8JsonWriter` had written a property name prior to the stream scope, it would expect to write a value next, if we write a property next, maybe it would send `Utf8JsonWriter` in an invalid state. If we disable `Utf8JsonWriter` validation, this would not throw an error, but I don't know if it would lead to unexpected behaviour down the line as a result of the `Utf8JsonWriter` being in an invalid state. Here are illustrative steps to make this process clearer:

- `ODataWriter.WriteStart(new ODataStreamPropertyInfo { Name = "MyProperty", ..., })`:
    - calls `IJsonWriter.WriteName("MyProperty")`
    - This eventually calls `utf8Writer.WritePropertyName("MyProperty")`
    - It writes the property name to the buffer then writes the property separator `:`
    - The `Utf8JsonWriter` remembers that we're currently on `PropertyName` token, inside an object.
- `ODataWriter.CreateBinaryWriteStream`
    - calls `IJsonWriter.StartStreamValueScope()`
    - first we call `utf8JsonWriter.Flush()` to ensure its data is written to the underlying stream before we write to the stream directly
    - write the opening `"` quote as a utf8 byte to the stream (`Utf8JsonWriter` is not aware of this write)
    - create a stream wrapper similar to `ODataBinaryWriterStream` that encodes its bytes to base64 string, but then writes them as utf8 encoded bytes to the underlying stream
    - return the stream to the caller
- `ODataWriter.WriteEnd()`
    - calls `IJsonWriter.EndStreamValueScope()`
    - writes the closing quote `"` as utf8 byte to the stream
    - flushes `ODataBinaryWriterStream`'s data to the underlying stream to ensure future `Utf8JsonWriter` writes are added to the stream after

It's important to note that after these steps, `Utf8JsonWriter` will still think it has just written a property name but no value. So it expects the next write to be the value of the "MyProperty" property. However, as far as `ODataWriter` is concerned, we've just written a value (the stream value). So it's valid to call `ODataWriter.Start(new ODataProperty ...)` to write a new property, which eventually calls `Utf8JsonWriter.WritePropertyName`. Writing a property name directly after another is invalid. We should verify that this won't cause unexpected results if we turn of validation.

This would work if we're writing the stream data as the value of a property. But what if we're writing the value as an item in the array? `Utf8JsonWriter` sets a flag after writing values to indicate that the next item should write a list separator first. If `Utf8JsonWriter` is not aware of the write we've just made, it would not add the list separator for the next item, this can result in invalid JSON. **TODO**: Find workaround for this, maybe `Utf8JsonWriter.WriteRawValue("")` would work, the idea is that it would output nothing, but move the `Utf8JsonWriter` in the correct state to account for the stream data that has just been written.

Note that the `ODataWriter.CreateBinaryWriteStream`'s implementation doesn't actually return the stream from `IJsonStreamWriter.StartStreamValueScope` directly, it wraps it inside an `ODataBinaryStreamWriter` stream which adds some buffering logic. It also ensures that the value stream is not disposed when the user disposes the `ODataBinaryStreamWriter`. This is the desired behaviour since `JsonWriter` handles the disposal of the stream it creates. I don't think this class should affect the implementation of `IJsonWriter.StartStreamValueScope()`, but I think it's worth noting. I also think that the implementation of `ODataBinaryStreamWriter` has a lot of room for optimizations that we should consider (e.g. avoiding excessive LINQ usage, avoiding allocation of temporary arrays, increasing the buffer size/min bytes per write event, using the array pool, etc.)

### Notes on providing a TextWriter for direct text writing

`JsonWriter` implements a `StartTextWriterValueScope` method which returns a `TextWriter`. This is conceptually similar to `StartStreamValueScope` except that it lets the user write text instead of data. It poses a similar challenge as `StartStreamValueScope` to implement with `Utf8JsonWriter` since it lets the user stream the data one chunk at a time instead of writing the whole value in a single method call. [Here's a test that demonstrates how it's used](https://github.com/OData/odata.net/blob/6643a9ef12388b07a46f3b460576e215ceb4d9b8/test/FunctionalTests/Microsoft.OData.Core.Tests/JsonLight/ODataJsonLightOutputContextApiTests.cs#L272).

To implement this we can follow the same pattern proposed for `StartStreamValueScope()`, but take into account the following key differences:
- The `TextWriter` returned by `JsonWriter` is an `ODataJsonTextWriter`. It doesn't convert the data to base64.
- It writes the plain text directly to the underlying text writer, but it escapes the text
- It uses `ODataStringEscapeOption.EscapeOnlyControls` instead of `EscapeNonAscii`. I'm not sure why it uses a different escaping mode, but we should honor it in our implementation. I don't know if `Utf8JsonWriter` provides a `TextEncoder` that implements the same escaping mechanisms.
- To implement this, we can provide a text writer whose write methods escape the text, utf8 transcode the text then write the bytes to the underlying stream

### Notes on Encoding and issues with TextWriter coupling

`Utf8JsonWriter` writes UTF-8 encoded json bytes directly to the stream when flushed. C# `char` are 16bit utf-16 characters. `Utf8JsonWriter` transcodes `string` and `char` inputs to UTF-8 first writing them. So writing strings or chars directly to the stream would not be compatible with `Utf8JsonWriter`. For workarounds that require writing directly to the Stream instead of passing through `Utf8JsonWriter`, we'd have to ensure the values we write pass through a `Utf8` encoder. [`Utf8Encoding`](https://docs.microsoft.com/en-us/dotnet/api/system.text.utf8encoding.getbytes?view=net-6.0) is a utility class that provides methods for encoding to and from UTF-8.

This poses a challenge for supporting a constructor that passes `TextWriter` directly. If the caller passes a stream to `Utf8ODataJsonWriter`, they expect the output to be written through that `TextWriter`. Since we don't have access to the `TextWriter`'s underlying stream, we can create a new `MemoryStream` and pass that to `Utf8JsonWriter`. Then we flush the `Utf8JsonWriter`, we write the data in the stream to the output `TextWriter`. But there's a problem, `TextWriter` does not expose methods for writing bytes directly, the `TextWriter` expects strings/chars as input or values that can be converted to strings. Remember that C# string/chars are UTF-16-based. So in order to move data from the stream to the `TextWriter` we'd have to decode the UTF8 bytes from the stream to strings first.

`JsonWriter` doesn't control the encoding of the output `TextWriter` it receives. Currently, the encoding is being determined by `ODataJsonFormat.DetectPayloadKind()` which returns an encoding based on the content type charset. The default encoding is UTF8, and this probably what's used in most (if not all) cases in production. If the `TextWriter`'s encoding is UTF8, then that also means it has to transcode the data written to it from UTF16 to UTF8. This means we'd be transcoding from utf16->utf8->utf16->utf8, which is wasteful and would degrade performance.

If we can ensure that for JSON payloads, this is the only encoding that is supported by OData, then that would make life easier, and would contribute to the case for passing a `Stream` to `IJsonWriter` instead of a `TextWriter`. If we need to support multiple output encoding, maybe we could a layer that pipes the stream passed to `IJsonWriter` through a UTF8-To-TargetEncoding transcoder.

### Notes on buffering

`Utf8JsonWriter` writes all data to a buffer and only writes to the stream when flushed. `Utf8JsonWriter` either takes a `Stream` as input, or an `IBufferWriter<byte>`. `IBufferWriter<T>` exposes methods to retrive the memory to write to as a `Span<T>` or `Memory<T>`. Then you write directly to those memory blocks and notify the `IBufferWriter<T>` that you've written data by calling `Advance()`. When you pass a `Stream` to the `Utf8JsonWriter` it will create an [`ArrayBufferWriter<byte>`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraybufferwriter-1?view=net-6.0) wrapper around the stream. `ArrayBufferWriter<T>` is built-in implementation of `IBufferWriter<T>` that allocates an array and uses it as the backing for the `IBufferWriter.GetMemory/GetSpan` methods. The array will be resized if more memory is requested than available. `JsonSerializer` uses a more efficient implementation of `IBufferWriter<T>` calls [`PooledByteBufferWriter`](https://source.dot.net/#System.Text.Json/PooledByteBufferWriter.cs,d0112696348c0f6d,references). This implementation does not allocate arrays directly, it rents them from the share `ArrayPool`, therefore cutting down on memory allocations by re-cycling arrays from the pool. This class is however internal and not exposed publicly. You can find more information about the decision to keep it internal on [this thread](https://github.com/dotnet/runtime/issues/33598). If the proposed implementations of `Utf8ODataJsonWriter` seem promising, we could consider also create a clone implementation of `PooledByteBufferWriter` and using it instead of the default.

### Pseudocode

Here's sample pseudocode for what the `Utf8ODataJsonWriter` could look like:

```c#

class Utf8ODataJsonWriter: IJsonStreamWriter, IJsonStreamWriterAsync, IDisposable {
    private Stream outputStream;
    private Utf8JsonWriter writer;

    public Utf8ODataJsonWriter(Stream outputStream, bool isIeee754Compatible, int maxDepth)
    {
        this.outputStream = outputStream;
        this.writer = new Utf8JsonWriter(outputStrean,
            new JsonWriterOptions { SkipValidation = true, MaxDepth = maxDepth });
    }

    public void WriteName(string name)
    {
        writer.WritePropertyName(name);
        FlushIfBufferThresholdReached();
    }

    public void WriteValue(bool value)
    {
        writer.WriteBooleanValue(value);
        FlushIfBufferThresholdReached();
    }

    private void FlushIfBufferThresholdReached()
    {
        if (writer.BytesPending > 0.9f * MaxBufferSize)
        {
            writer.Flush();
        }
    }

    public void StartObjectScope()
    {
        writer.WriteStartObject();
        FlushIfBufferThresholdReached();
    }

    public void EndObjectScope()
    {
        write.WriteEndObject();
        FlushIfBufferThresholdReached();
    }

    public void StartArrayScope()
    {
        writer.WriteStartArray();
        FlushIfBufferThresholdReached();
    }

    public void EndArrayScope()
    {
        writer.WriteEndArray();
        FlushIfBufferThresholdReached();
    }

    public void WriteFunctionPaddingName(string functionName)
    {
        // write directly to stream
        outputStream.Write(Utf8Encode(functionName));
    }

    public void StartFunctionPaddingScope()
    {
        outputStream.Write(Utf8Encode('(')));
    }

    public void EndFunctionPaddingScope()
    {
        outputStream.Write(Utf8Encode(')')));
    }

    public Stream StartStreamValueScope()
    {
        // flush pending contents in writer to ensure
        // we don't interleave writes
        writer.Flush();
        outputStream.Write(UtfEncode('"'));
        this.binaryValueStream = new ODataUtf8BinaryWriterStream(outputStream);
        return this.binaryValueStream;
    }

    public Stream EndStreamValueScope()
    {
        this.binaryValueStream.Flush();
        // this should not dispose the underlying stream
        this.binaryValueStream.Dispose();
        this.binaryValueStream = null;
        this.outputStream.Write(Utf8Encode('"'));
    }

    public TextWriter StartTextWriterValueScope(string contentType)
    {
        writer.Flush();
        this.currentContentType = contentType;
        if (!IsWritingJson) // thich checks whether the contentType is non-json
        {
            // if it's non-json, then treat the contents to be written as
            // as a string value to escape
            outputStream.Write(Utf8Encode('"'));
            this.textWriter = ODataUtf8JsonTextWriter(outputStream);
            return this.textWriter;
        }

        // if the content type is json, it's the responsibility
        // of the caller to write valid JSON and escape it
        // this writer should not dispose the underlying stream
        // I think a simple implementation would be:
        // new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: true)
        this.textWriter = new ODataUtf8TextWriter(outputStream);
        return writer;
    }

    public TextWriter EndTextWriterValueScope()
    {
        this.textWriter.Flush();
        this.textWriter.Dispose();
        this.textWriter = null;
        if (!IsWritingJson)
        {
            outputStream.Write(Utf8Encode('"'));
        }
    }

    // ASYNC
    public async Task WriteNameAsync(string name)
    {
        writer.WritePropertyName(name);
        await FlushIfBufferThresholdReachedAsync();
    }

    public async Task WriteValue(bool value)
    {
        writer.WriteBooleanValue(value);
        await FlushIfBufferThresholdReachedAsync();
    }

    // since we expect this not to flush most of the times
    // maybe it's a good candidate to use ValueTask:
    // https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/
    private Task FlushIfBufferThresholdReachedAsync()
    {
        if (writer.BytesPending > 0.9f * MaxBufferSize)
        {
            await writer.FlushAsync();
        }
    }
}
```

## 2. Removing Async API from IJsonWriter implementation and requiring caller to Flush manually

This is an evolution of the previous proposal and assumes that some `ODataUtf8JsonWriter` implementation has been demonstrated to be feasible. The proposed `ODataUtf8JsonWriter` so far potentially calls `Utf8JsonWriter.Flush/FlushAsync()` on every write call. In most cases it won't actually call `Flush/Async()` since we expect most writes to be buffered in memory. Even though most of the calls won't flush, we still have async versions of all the methods so that when they need to flush, they call `FlushAsync()` instead of `Flush`. This sounds like it would lead to unnecessary async overhead for calls that will write synchronously to memory most of the time. The aim of this proposal is to see whether we can minimize this overhead and whether that would beneficial to the overall writer's performance.

Maybe using [ValueTask](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/) would reduce some of the overhead. But I think we can go a step further.

We can take inspiration from the design of [`JsonSerializer.SerializeAsync`](https://source.dot.net/#System.Text.Json/System/Text/Json/Serialization/JsonSerializer.Write.Stream.cs,246). It calls a synchronous `Write***()` method inside a loop which writes to the `IBufferWriter`'s memory. After each iteration it checks how much data has been buffered, and if it exceeds a threshold, then it calls `bufferWriter.WriteToStreamAsync()` and clear the buffer. Similarly, we can remove the flush calls from `ODataUtf8JsonWriter` and keep it's API synchronous. We would still have to keep the `Stream` and `TextWriter` related methods async since they write directly to the stream. But the most common cases will be synchronous. Specifically, we would remove the `FlushIfBufferThresholdReached` method. Then it would be the responsibility of the caller to flush the data to the stream.

`ODataWriter`'s design is not as simple as `JsonSerializer`, since we expose granualar `WriteStart` and `WriteEnd` methods. So we have to think more carefully about where and when flush. Conceptually, we'd be moving the `FlushIfBufferThresholdReached` method to upper levels of the call stack. But where to?

A naive approach would be to remove that method from the `ODataMessageWriter`'s call stack altogether, i.e. the core writer would not be concerned about when to flush (except for cases where we flush the text writer). This would simplify the design of `ODataMessageWriter` (it could even make most async methods obsolete). However, it will push the complexity of handling the flushing decision to the public API. The `ODataSerializer` in WebAPI and the quivalent serializer in the Client would have to be refactored to flush. Users who use OData core directly would have to make this decision themselves. All-in-all this would introduce an extra layer of complexity to library consumers, in addition to being a major breaking change. It's better for us to handle this complexity internally.

So the ideal approach is to deal with this complexity internally and keep the public API of `ODataMessageWriter` and `ODataWriter` intact. `ODataJsonLightWriter` could call `FlushIfBufferThresholdReached` at key "checkpoints" where it may have written a sizeable chunk of data. For example, it may not make much sense to call that method after calling `IJsonWriter.StartObjectScope()` since that only writes a single character. But it may make sense to call it after writing propert+value pair, `IJsonWriter.EndObjectScope()` or `EndArrayScope()` since it's likely more data would have been written at this point. Find the optimal spots may require running a number of tests with different variations and comparing results. For the purpose of this experiment, we can add the `FlushIfBufferThresholdReached()` call to the `End***` methods, e.g.:

```c#
class ODataJsonLightWriter
{
    protected override void EndResource(ODataResource resource)
    {
        /* ... */

        FlushIfBufferThresholdReached();
    }
    protected override async Task EndResourceAsync(ODataResource resource)
    {
        /* ... */

        await FlushIfBufferThresholdReachedAsync();
    }
}
```

The danger in buffering too infrequently is that we can exceed the memory buffer and have to resize it, technically it may also be possible to run out of memory if we write an object with an unreasonable number of properties, or array with too many elements, or if we write an excessively long string value. Utf8JsonWriter throws an out-of-memory exception whe requesting a buffer whose length is large than the int max size. [Here's a document](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/docs/ThreatModel.md) that provides insights on some of the security considerations in System.Text.Json.

## 3. Using a shared `IBufferWriter<byte>` for both `Utf8JsonWriter` and `Stream`/`TextWriter` APIs

The proposed `Utf8ODataJsonWriter` implementation requires special handling in the `StartStreamValueScope()` and `StartTextWriterValueScope()` methods, namely that we have to bypass the `Utf8JsonWriter` and write directly to the stream. This presents the following drawbacks:
- We have to flush `Utf8JsonWriter` to ensure the data from the return `TextWriter` or `Stream` is not written to the output stream in the wrong order
- We have to do manual UTF8 transcoding
- We have to write single quote characters (`"`) directly to the stream, which may be inefficient
- We have to make each of these methods async before of the direct stream writes and flushes. This also makes the API inconsistent due to some methods being only sync and some being async

I think we can address some of these challenges by controlling the `IBufferWriter<byte>` directly. Instead of passing a `Stream` to `Utf8JsonWriter`, we pass an `IBufferWrite<byte>` directly, either the default `ArrayBufferWriter<byte>` or a custom clone of `PooledByteBufferWriter`.

Now instead of writing the enclosing quotes to the stream, we write them to the `IBufferWrite<byte>`'s memory synchronously. We also don't have to force a flush for correctness (unless it's beneficial for performance, e.g. to avoid resizes).

The custom `ODataBinaryStreamWriter` and `ODataJsonTextWriter` will have to write the `IBufferWrite<byte>` instead of the `Stream`. This will be a non-trivial refactor because working with an `IBufferWriter<T>` is a significantly different programming model from writing to a `Stream` directly. `Utf8JsonWriter.Flush()` also behaves differently when you pass a stream to it. Careful attention to detail would be required to ensure overall correctness. Here's a list of some of the new challenges that this approach presents:

- `IBufferWriter` doesn't expose any methods to write or flush to the stream
- `Utf8JsonWriter.Flush()` doesn't actually flush the `IBufferWriter`, it merely `Advance`s it and updates the `BytesPending` and `BytesCommitted` values
- This means that we'll still need to call `Utf8JsonWriter.Flush()` before writing directly to the `IBufferWriter` in order to maintain correctness, but this won't flush to the stream and so can be done synchronously
- To "Flush" the buffer to the stream we'll have to:
    - Get the data written to the buffer so far from `ArrayBufferWriter.WrittenSpan` (or `WrittenMemory` in async methods)
    - Write the memory's contents to the stream
    - Clear the buffer `ArrayBufferWriter.Clear()`
    - We should ensure to `Utf8JsonWriter.Flush()` is called before we "flush" the buffer writer to ensure the "pending" bytes are also flushed and reflect as written to the buffer
- We should use `ArrayBufferWriter.WrittenCount` to find out how many bytes are in the buffer and not `Utf8JsonWriter.BytesPending`
- `ODataBinaryStreamWriter.Write` methods will have to ensure the base64-encoded bytes are also utf8 encoded before writing to the buffer writer
- To prevent a large file or other stream from being entirely buffered in memory, `ODataBinaryStreamWriter` should also regularly call `FlushIfBufferThresholdReached()`, maybe after a certian number of written bytes. But the caller can also force a flush by calling `Flush/FlushAsync()` on the returned stream.
- `ODataJsonTextWriter` will also have to occasionally flush its writes to the stream
- Writing to the buffer involves getting some memory from it using `buffer.GetSpan/GetMemory()`, writing to it then calling `Advance()` to notify the buffer write that data has been written. If writing from `ODataBinaryStreamWriter` or `ODataJsonTextWriter` we have to manage the buffering: what size of memory to request, extending it if we exceed the size, etc. There's a helper extension method [`BuffersExtensions.Write`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.buffersextensions.write?view=net-6.0) that can abstract some of the details from writing to an `IBufferWriter` directly.

## Pseudocode

```c#
class Utf8ODataJsonWriter: IJsonStreamWriter, IJsonStreaWriterAsync, IDisposable
{
    public Utf8ODataJsonWriter(Stream outputStream, bool isIeee754Compatible, int maxDepth)
    {
        this.outputStream = outputStream;
        this.bufferWriter = new ArrayBufferWriter(Capacity);
        this.utf8Writer = new Utf8JsonWriter(this.bufferWriter, new JsonWriterOptions { SkipValidation = true, MaxDepth = maxDepth });
    }

    public async Task FlushAsync()
    {
        if (utf8Writer.PendingBytes > 0)
        {
            utf8Writer.Flush();
        }
    
        var bufferedBytes = this.bufferWriter.WrittenSpan;
        await outputStream.WriteAsync(bufferedBytes);
        await outputStream.FlushAsync(); // not sure if this is necessary
        bufferWriter.Clear();
    }

    public async Task FlushIfBufferThresholdReachedAsync()
    {
        if (utf8Writer.PendingBytes > 0)
        {
            utf8Writer.Flush();
        }

        if (bufferWriter.WrittenCount >= 0.9f * BufferSize)
        {
            await FlushAsync();
        }
    }

    // this will be called by `ODataUtf8BinaryWriterStream`
    // and `ODataUtf8TextWriter`
    internal async Task WriteAsync(ReadOnlySpan<byte> buffer)
    {
        bufferWriter.Write(buffer); // uses BufferExtensions.Write helper
        await FlushIfBufferThresholdReachedAsync();
    }


    // this method does not need to be async
    public Stream StartStreamValueScope()
    {
        // commit pending bytes in Utf8JsonWriter to the IBufferWriter
        writer.Flush();
        bufferWriter.Write(UtfEncode('"'));
        this.binaryValueStream = new ODataUtf8BinaryWriterStream(this);
        return this.binaryValueStream;
    }

    // this does not need to be async
    public Stream EndStreamValueScope()
    {
        // we don't need to flush since we have been writing
        // also binaryValueStream.Write() will have been flushing
        // occasionally to the stream
        this.binaryValueStream.Flush();
        // this should not dispose the underlying stream
        this.binaryValueStream.Dispose();
        this.binaryValueStream = null;
        this.outputStream.Write(Utf8Encode('"'));
    }
}
```

# 4. Moonshot: Generate strongly-typed converters from model to avoid boxing

# 5. Moonshot: Generate strongly-type model-based serializers at compile-time






