# Experiment proposals

This is a list of experiments that I think we should explore and evaluate to help reach a conclusion or recommendation about whether and how we should integrate `Utf8JsonWriter`, what the potential gains would be and what tread-offs should be considered.

The experiments are listed in ascending order of the perceived difficulty in implementing. The potential performance gains are also expected to increase. We should assess both the performance again as well the amount of effort it would take to implement that proposal as well as any thing we'd have to sacrifice (e.g. features/flexibility, etc.). Some proposals may require breaking changes to the public API.

## Wrap the synchronous writer in a `BufferedStream`

The `ODataJsonLightOutputContext` wraps the output `Stream` in an `AsyncBufferedStream` to provide some buffering when in async mode. But in synchronous mode, the stream is not buffered. In a [recent PR]() the custom `AsyncBufferedStream` class was replaced with the standard library's new `BufferedStream` which resulted in a peformance increase. It is not clear to me why in the synchronous mode the `BufferedStream` is not buffered.

One quick thing to try would be to wrap the sync `Stream` in a `BufferedStream` and check whether that improves perf. This is a low-effort experiment that does not require any breaking changes.

I am also curious to know what the performance for the async scenario would be without the `BufferedStream`.

## Implement a custom `IJsonWriter` using `Utf8JsonWriter` and inject it via dependency injection

The OData writer allows injecting a custom `IJsonWriter` implementation. To do this, we have to create a class that implements `IJsonWriterFactory` then inject that in the service container. To provide dependency injection capabilities the `IODataRequestMessage`/`IODataResponseMessage` should also implement `IContainerProvider`. This means it should expose an `Container` property which is the actual `IServiceProvider`.

`IJsonWriter` and `Utf8JsonWriter` operate at similar level of granularity (writing properties and primitive values, starting/ending object and arrays, etc.) so implementing `IJsonWriter` using `Utf8JsonWriter` should be possible. However, there are couple of complications:
- `IJsonWriter` is expected to write to a `TextWriter` as opposed to a stream directly (the `IJsonWriterFactory.CreateJsonWriter` expects a `TextWriter` as paramter). `Utf8JsonWriter` either takes a `Stream` or an `IBufferWriter` as input.
- OData provides both `IJsonWriter` and `IJsonWriterAsync` APIs. These declare sync and async versions of the same methods. The default `JsonWriter` implements both interfaces and is used for writing both synchronously and asynchronously. `Utf8JsonWriter` only has synchronous write methods. It writes synchronously to the `IBufferWriter`'s memory and only sends the data to the stream when you `Flush()` or `FlushAsync()`.
- `JsonWriter` also implements `IJsonStreamWriter` which expose methods that return a `Stream` or `TextWriter` to the caller, allowing the call to write output directly to those streams. This is used when writing binary data in a streaming fashion, e.g. piping from one stream to another without reading all the bytes from the input stream into memory in advance. The stream is wrapped in an `ODataBinaryStreamWriter` whichs does a bit of buffering, the result is eventually converted to to base64 before being written to the final destination by `JsonWriter` (TODO: confirm this)

If we create a custom `IJsonWriter` implementation, we'd have to workaround those constraints and make them fit even if it's not the most optimal way of using a `Utf8JsonWriter`, the benefit would be that integration with OData writer would be "plug-and-play" and not require any breaking changes. The tight coupling between `IJsonWriter` and `TextWriter` complicates a potential integration unreasonably. For this reason I will first propose an implementation that does not require `TextWriter` then propose how we could workaround those to make things work with `TextWriter`. While removing the `TextWriter` coupling would be a breaking change, I still think this proposal is worthwhile to look at first because:
- The `TextWriter` coupling should be reconsidered whether or not we decide to integrate `Utf8JsonWriter`. I strongly propose that we remove this requirement in OData Core 8.x, it makes the `IJsonWriter` less flexible.
- It would be relatively easy to replace the `TextWriter` parameter in `IJsonWriterFactory` with `Stream`. I don't think there are many (if any) users of OData who actually provide a custom `IJsonWriterFactory` anyway. And if there are, they could pass the underlying the stream of the `TextWriter` directly instead.
- For the `StartTextWriterScope()` method which returns a `TextWriter`, we could change the implementation to wrap the TextWriter around the stream we now have instead of the `TextWriter`