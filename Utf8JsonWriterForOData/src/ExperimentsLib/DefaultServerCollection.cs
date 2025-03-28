﻿using Microsoft.OData.Edm;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ExperimentsLib;

namespace ExperimentsLib
{
    public static class DefaultServerCollection
    {
        const int BufferSize = 84000;
        public static ServerCollection<IEnumerable<Customer>> Create(IEnumerable<Customer> data, string baseHost="http://localhost")
        {
            IEdmModel model = DataModel.GetEdmModel();
            model.MarkAsImmutable();
            ServerCollection<IEnumerable<Customer>> servers = new(data, baseHost);

            servers.AddServers(
                ("JsonSerializer", "utf-8", new JsonSerializerServerWriter()),

                ("Utf8JsonWriter-Direct", "utf-8", new Utf8JsonWriterBasicServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter-Direct-NoValidation", "utf-8", new Utf8JsonWriterBasicServerWriter(
                    stream => new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }))),
                ("Utf8JsonWriter-Direct-ResourceGeneration-NoValidation", "utf-8", new Utf8JsonWriterBasicServerWriter(
                    stream => new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }), simulateResourceGeneration: true)),
                ("Utf8JsonWriter-Direct-ArrayPool-NoValidation", "utf-8", new Utf8JsonWriterBasicServerWriterWithArrayPool(
                    bufferWriter => new Utf8JsonWriter(bufferWriter, new JsonWriterOptions { SkipValidation = true }))),
                ("Utf8JsonWriter-Direct-ArrayPool-ResourceGeneration-NoValidation", "utf-8", new Utf8JsonWriterBasicServerWriterWithArrayPool(
                    bufferWriter => new Utf8JsonWriter(bufferWriter, new JsonWriterOptions { SkipValidation = true }), simulateResourceGeneration: true)),

                ("Utf8JsonWriter", "utf-8", new Utf8JsonWriterServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter-NoValidation", "utf-8", new Utf8JsonWriterServerWriter(stream =>
                    new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }))),
                ("Utf8JsonWriter-ArrayPool", "utf-8", new Utf8JsonWriterServerWriterWithArrayPool(bufferWriter => new Utf8JsonWriter(bufferWriter))),
                ("Utf8JsonWriter-ArrayPool-NoValidation", "utf-8", new Utf8JsonWriterServerWriterWithArrayPool(
                    bufferWriter => new Utf8JsonWriter(bufferWriter, new JsonWriterOptions { SkipValidation = true }))),

                ("ODataJsonWriter-Direct", "utf-8", new ODataJsonWriterBasicServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriter())),
                ("ODataJsonWriter-Direct-Buffered", "utf-8", new ODataJsonWriterBasicServerWriter(
                    stream => new BufferedStream(stream, BufferSize).CreateUtf8ODataJsonWriter())),
                ("ODataJsonWriter-Direct-ResourceGeneration", "utf-8", new ODataJsonWriterBasicServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriter(), simulateTypedResourceGeneration: true)),
                ("ODataJsonWriter-Direct-ResourceGeneration-Buffered", "utf-8", new ODataJsonWriterBasicServerWriter(
                    stream => new BufferedStream(stream, BufferSize).CreateUtf8ODataJsonWriter(), simulateTypedResourceGeneration: true)),

                ("ODataJsonWriter-Direct-Async", "utf-8", new ODataJsonWriterAsyncBasicServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriterAsync())),
                ("ODataJsonWriter-Direct-ResourceGeneration-Async", "utf-8", new ODataJsonWriterAsyncBasicServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriterAsync(), simulateTypedResourceGeneration: false)),

                ("ODataJsonWriter-Utf8", "utf-8", new ODataJsonWriterServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriter())),
                ("ODataJsonWriter-Utf16", "utf-16", new ODataJsonWriterServerWriter(stream => stream.CreateUtf16ODataJsonWriter())),
                ("ODataJsonWriter-Utf8-Buffered", "utf-8", new ODataJsonWriterServerWriter(
                    stream => new BufferedStream(stream, BufferSize).CreateUtf8ODataJsonWriter())),

                ("ODataJsonWriter-Utf8-Async", "utf-8", new ODataAsyncJsonWriterServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriterAsync())),
                ("ODataJsonWriter-Utf16-Async", "utf-16", new ODataAsyncJsonWriterServerWriter(
                    stream => stream.CreateUtf16ODataJsonWriterAsync())),
                ("ODataJsonWriter-Utf8-Buffered-Async", "utf-8", new ODataAsyncJsonWriterServerWriter(
                    stream => new BufferedStream(stream, BufferSize).CreateUtf8ODataJsonWriterAsync())),

                ("NoOpWriter", "utf-8", new ODataJsonWriterServerWriter(
                    stream => new NoopJsonWriter())),
                ("NoOpWriter-Async", "utf-8", new ODataAsyncJsonWriterServerWriter(stream => new NoopJsonWriter())),

                ("ODataMessageWriter-Utf8", "utf-8", new ODataServerWriter(model, stream => stream.CreateUtf8Message())),
                ("ODataMessageWriter-Utf16", "utf-16", new ODataServerWriter(model, stream => stream.CreateUtf16Message())),
                ("ODataMessageWriter-Utf8-Buffered", "utf-8", new ODataServerWriter(model, stream => new BufferedStream(stream, BufferSize).CreateUtf8Message())),
                ("ODataMessageWriter-Utf8-NoValidation", "utf-8", new ODataServerWriter(model, stream => stream.CreateUtf8Message(), enableValidation: false)),
                ("ODataMessageWriter-NoOp", "utf-8", new ODataServerWriter(model, stream => stream.CreateNoopMessage())),
                ("ODataMessageWriter-NoOp-NoValidation", "utf-8", new ODataServerWriter(model, stream => stream.CreateNoopMessage(), enableValidation: false)),

                ("ODataMessageWriter-Utf8-Async", "utf-8", new ODataAsyncServerWriter(model, stream => stream.CreateUtf8Message())),
                ("ODataMessageWriter-Utf16-Async", "utf-16", new ODataAsyncServerWriter(model, stream => stream.CreateUtf16Message())),
                ("ODataMessageWriter-Utf8-NoValidation-Async", "utf-8", new ODataAsyncServerWriter(model, stream => stream.CreateUtf8Message(), enableValidation: false)),
                ("ODataMessageWriter-NoOp-Async", "utf-8", new ODataAsyncServerWriter(model, stream => stream.CreateNoopMessage())),
                ("ODataMessageWriter-NoOp-NoValidation-Async", "utf-8", new ODataAsyncServerWriter(model, stream => stream.CreateNoopMessage(), enableValidation: false))
                );

            return servers;
        }
    }
}
