# Experiment proposals

This is a list of experiments that I think we should explore and evaluate to help reach a conclusion or recommendation about whether and how we should integrate `Utf8JsonWriter`, what the potential gains would be and what tread-offs should be considered.

The experiments are listed in ascending order of the perceived difficulty in implementing. The potential performance gains are also expected to increase. We should assess both the performance again as well the amount of effort it would take to implement that proposal as well as any thing we'd have to sacrifice (e.g. features/flexibility, etc.). Some proposals may require breaking changes to the public API.

## 1. Wrap the synchronous writer in a `BufferedStream`

The `ODataJsonLightOutputContext` wraps the output `Stream` in an `AsyncBufferedStream` to provide some buffering when in async mode. But in synchronous mode, the stream is not buffered. In a [recent PR](https://github.com/OData/odata.net/pull/2222) the custom `AsyncBufferedStream` class was replaced with the standard library's new `BufferedStream` which resulted in a peformance increase. It is not clear to me why in the synchronous mode the `BufferedStream` is not buffered.

One quick thing to try would be to wrap the sync `Stream` in a `BufferedStream` and check whether that improves perf. This is a low-effort experiment that does not require any breaking changes.

I am also curious to know what the performance for the async scenario would be without the `BufferedStream`.

## 2. Implement a custom `IJsonWriter` using `Utf8JsonWriter` and inject it via dependency injection

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

### Notes on string escaping

`JsonWriter` escapes property names and all string values written with `WriteValue(string value)` and `WriteName(string name)`. Values written with `WriteRawValue(string value)` or most string values from conversions of other primitive types. However, when in `Ieee754Compatible` mode, where `long`s and `double`s are quoted, they are also escaped.

By default, OData uses the `ODataStringEscapeOption.EscapeNonAscii` which escapes all control characters (like `\n`) and non-ascii characters. `Uf8JsonWriter` allows you to specify a `TextEncoder` which will handle the escaping. When none is provided, it will use a default `JavaScriptEncoder` which also escapes non-ascii chars(outside basic-latin Unicode range). So we can rely on `Utf8JsonWriter`'s escaping instead of using our own (much less efficient) implementation.
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

To implement this in `Utf8ODataJsonWriter` would be a bit tricky since `Utf8JsonWriter` does not provide a way to write a JSON value one chunk at a time, and it doesn't expose methods to write to the stream directly. A naive approach would be to return a memory stream, then once the `EndStreamValueScope` is called, we convert the bytes to a base64 string and call `WriteRawValue()` in one chunk. This would be problematic if the data to write is too large or inefficient to keep in memory all at once. Another approach is to write the chunks directly to the underlying stream, bypassing `Utf8JsonWriter`. But if `Utf8JsonWriter` had written a property name prior to the stream scope, it would expect to write a value next, if we write a property next, maybe it would send `Utf8JsonWriter` in an invalid state (TODO: verify what happens when you write invalid JSON after disabling validation).

### Notes on Encoding and issues with TextWriter coupling

`Utf8JsonWriter` writes utf-8 encoded json bytes directly to the stream when flushed. C# `char` are 16bit utf-16 characters, so writing strings or chars directly to the stream would not be compatible with `Utf8JsonWriter`. For workarounds that require writing directly to the Stream instead of passing through `Utf8JsonWriter`, we'd have to ensure the values we write pass through a `Utf8` encoder. [`Utf8Encoding`](https://docs.microsoft.com/en-us/dotnet/api/system.text.utf8encoding.getbytes?view=net-6.0) is a utility class that provides methods for encoding to and from UTF-8.

This poses a challenge for supporting a constructor that passes `TextWriter` directly. If the caller passes a stream to `Utf8ODataJsonWriter`, they expect the output to be written through that `TextWriter`. Since we don't have access to the `TextWriter`'s underlying stream, we can create a new `MemoryStream` and pass that to `Utf8JsonWriter`. Then we flush the `Utf8JsonWriter`, we write the data in the stream to the output `TextWriter`. But there's a problem, `TextWriter` does not expose methods for writing bytes directly, the `TextWriter` expects strings/chars as input or values that can be converted to strings. Remember that C# string/chars are UTF-16-based. So in order to move data from the stream to the `TextWriter` we'd have to decode the UTF8 bytes from the stream to strings first.

`JsonWriter` doesn't control the encoding of the output `TextWriter` it receives. Currently, the encoding is being determined by `ODataJsonFormat.DetectPayloadKind()` which returns an encoding based on the content type charset. The default encoding is UTF8, and this probably what's used in most (if not all) cases in production. If the `TextWriter`'s encoding is UTF8, then that also means it has to encode the data written to it from UTF16 to UTF8. This means we'd have an unnecessary utf8->utf16->utf8 encoding flows, which is wasteful and would degrade performance.

If we can ensure that for JSON payloads, this is the only encoding that is supported by OData, then that would make life easier, and would corroborate the case for decoupling `IJsonWriter` from `TextWriter`