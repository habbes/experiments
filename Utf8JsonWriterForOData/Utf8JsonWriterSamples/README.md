# Getting familiar with Utf8JsonWriter

This experment compares writing an OData response using the `ODataMessageWriter` vs using `Utf8JsonWriter` directly (i.e. without using `JsonSerializer`). The goal is to get acquainted with the `Utf8JsonWriter` APIs and settings and to get a sense of how `Utf8JsonWriter` could potentially be used by `ODataWriter` internally.

Here are my take-aways:

## API

The API looks similar to OData's [`JsonWriter`](https://github.com/OData/odata.net/blob/master/src/Microsoft.OData.Core/Json/JsonWriter.cs).

- It contains methods for writing primitive properties in an object:
    - `WriteString(propertyName, value)`
    - `WriteNumber(propertyName, value)`
    - `WriteBoolean(propertyName, value)`
    - etc.
- methods for writiting primitive values in an array:
    - `WriteStringValue(value)`
    - `WriteNumberValue(value)`
    - etc.
- methods for starting object and array properties
    - `WriteStartObject(propertyName)`
    - `WriteStartArray(propertyName)`
- methods for starting object and array values:
    - `WriteStartObject()`
    - `WriteStartArray()`
- methods for closing arrays:
    - `WriteEndObject()`
    - `WriteEndArray()`

## Buffering

The `Utf8JsonWriter` writes directly to a buffer. It takes an optional [`IBufferWriter`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.ibufferwriter-1?view=net-6.0) as constructor argument. This where it writes to. the buffer writer exposes `GetMemory()` and `GetSpan()` functions that provides a buffer for the `Utf8JsonWriter` to write to, and an `Advance()` method used to tell the `IBufferWriter` how many bytes have been written to the memory buffer. To write the committed bytes to the output stream, you should `writer.Flush/Async()` method.

If you pass a `Stream` as argument instead of an `IBufferWriter`, the `Utf8JsonWriter` defaults to using an [`ArrayBufferWriter`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraybufferwriter-1?view=net-6.0) which backs the buffer with a heap-allocated array.

The `JsonSerializer` uses the internal [`PooledArrayBufferWriter`](https://source.dot.net/#Microsoft.AspNetCore.Mvc.ViewFeatures/PooledArrayBufferWriter.cs,75056dbb19cacf28) which rents buffers from the [`ArrayPool`]() instead of allocating directly.

## Async support

The `Utf8JsonWriter` has **no direct support for asynchronous writing**. All writing is performed synchronously on the buffer memory. To send the written data to the stream, `Flush()` must be called, and this can be performed asynchronously using [`FlushAsync`](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonwriter.flushasync?view=net-6.0#system-text-json-utf8jsonwriter-flushasync(system-threading-cancellationtoken)).

## Validation

## Keeping track of nested scope (utf8jsonwriter and JsonSerializer) vs OData

## Value conversion: how values are written? reflection? boxing?