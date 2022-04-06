using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.OData.Edm;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Utf8JsonWriterSamples;

namespace JsonWriterBenchmarks
{
    [MemoryDiagnoser]
    [EventPipeProfiler(EventPipeProfile.CpuSampling)]
    [SimpleJob(invocationCount: 10)]
    public class Benchmarks
    {
        IEnumerable<Customer> data;
        IEdmModel model;

        public Stream memoryStream;
        public Stream fileStream;

        // this is the buffer size used internally by OData
        // for buffering the async writer. It seems large
        // but we use it her for a fair comparison
        // JsonSerializer uses a default buffer size of 16*1024
        // maybe we can compare the 2
        const int BufferSize = 84000;

        // payload writers based on Utf8JsonWriter
        Utf8JsonWriterServerWriter utf8JsonPayloadWriter;
        Utf8JsonWriterServerWriter utf8JsonPayloadWriterNoValidation;

        // payload writers based on OData's internal JsonWriter
        ODataJsonWriterServerWriter jsonWriterPayloadWriterUtf8;
        ODataJsonWriterServerWriter jsonWriterPayloadWriterUtf16;
        ODataJsonWriterServerWriter jsonWriterPayloadWriterUtf8Buffered;
        // async versions
        ODataAsyncJsonWriterServerWriter jsonWriterPayloadWriterUtf8Async;
        ODataAsyncJsonWriterServerWriter jsonWriterPayloadWriterUtf16Async;
        ODataAsyncJsonWriterServerWriter jsonWriterPayloadWriterUtf8BufferedAsync;

        // payload writers based on the full ODataMessageWriter
        ODataServerWriter odataWriterPayloadWriterUtf8;
        ODataServerWriter odataWriterPayloadWriterUtf16;
        ODataServerWriter odataWriterPayloadWriterUtf8NoValidation;
        // ODataMessageWriter buffers the stream in async mode, but not in sync mode
        // we measure the sync writer with BufferedStream to see if it would improve perf
        ODataServerWriter odataWriterPayloadWriterUtf8Buffered;
        // the no-op writers don't write any output, they're meant to measure
        // OData's message processing and validation overhead
        ODataServerWriter odataWriterPayloadWriterNoOp;
        ODataServerWriter odataWriterPayloadWriterNoOpNoValidation;
        // async versions
        ODataAsyncServerWriter odataWriterPayloadWriterUtf8Async;
        ODataAsyncServerWriter odataWriterPayloadWriterUtf16Async;
        ODataAsyncServerWriter odataWriterPayloadWriterUtf8NoValidationAsync;
        ODataAsyncServerWriter odataWriterPayloadWriterNoOpAsync;
        ODataAsyncServerWriter odataWriterPayloadWriterNoOpNoValidationAsync;

        public Benchmarks()
        {
            // the written output will be about 1.45MB of JSON text
            data = CustomerDataSet.GetCustomers(5000);
            model = DataModel.GetEdmModel();

            // payload writers based on Utf8JsonWriter
            utf8JsonPayloadWriter = new Utf8JsonWriterServerWriter(
                stream => new Utf8JsonWriter(stream));
            utf8JsonPayloadWriterNoValidation = new Utf8JsonWriterServerWriter(
                stream =>new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }));

            // payload writers based on OData's internal JsonWriter
            jsonWriterPayloadWriterUtf8 = new ODataJsonWriterServerWriter(
                stream => stream.CreateUtf8ODataJsonWriter());
            jsonWriterPayloadWriterUtf16 = new ODataJsonWriterServerWriter(
                stream => stream.CreateUtf16ODataJsonWriter());
            jsonWriterPayloadWriterUtf8Buffered = new ODataJsonWriterServerWriter(
                stream => new BufferedStream(stream, BufferSize).CreateUtf16ODataJsonWriter());
            jsonWriterPayloadWriterUtf8Async = new ODataAsyncJsonWriterServerWriter(
                stream => stream.CreateUtf8ODataJsonWriterAsync());
            jsonWriterPayloadWriterUtf16Async = new ODataAsyncJsonWriterServerWriter(
                stream => stream.CreateUtf16ODataJsonWriterAsync());
            jsonWriterPayloadWriterUtf8BufferedAsync = new ODataAsyncJsonWriterServerWriter(
                stream => new BufferedStream(stream, BufferSize).CreateUtf8ODataJsonWriterAsync());

            // payload writers based on the full ODataMessageWriter
            odataWriterPayloadWriterUtf8 = new ODataServerWriter(model,
                stream => stream.CreateUtf8Message());
            odataWriterPayloadWriterUtf16 = new ODataServerWriter(model,
                stream => stream.CreateUtf16Message());
            odataWriterPayloadWriterUtf8NoValidation = new ODataServerWriter(model,
                stream => stream.CreateUtf8Message(), enableValidation: false);
            odataWriterPayloadWriterUtf8Buffered = new ODataServerWriter(model,
                stream => new BufferedStream(stream, 1024).CreateUtf8Message());
            odataWriterPayloadWriterNoOp = new ODataServerWriter(model,
                stream => stream.CreateNoopMessage());
            odataWriterPayloadWriterNoOpNoValidation = new ODataServerWriter(model,
                stream => stream.CreateNoopMessage(), enableValidation: false);
            // async versions
            odataWriterPayloadWriterUtf8Async = new ODataAsyncServerWriter(model,
                stream => stream.CreateUtf8Message());
            odataWriterPayloadWriterUtf16Async = new ODataAsyncServerWriter(model,
                stream => stream.CreateUtf16Message());
            odataWriterPayloadWriterUtf8NoValidationAsync = new ODataAsyncServerWriter(model,
                stream => stream.CreateUtf8Message(), enableValidation: false);
            odataWriterPayloadWriterNoOpAsync = new ODataAsyncServerWriter(model,
                stream => stream.CreateUtf8Message());
            odataWriterPayloadWriterNoOpNoValidationAsync = new ODataAsyncServerWriter(model,
                stream => stream.CreateUtf8Message(), enableValidation: false);

        }

        // -- In Memory Writes -- //
        #region InMemory

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task JsonSerializer_WriteMemory()
        {
            await JsonSerializer.SerializeAsync(memoryStream, data);
        }

        // Utf8JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task Utf8JsonWriter_WriteMemory() =>
            await WriteToMemory(utf8JsonPayloadWriter);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task Utf8JsonWriterNoValidation_WriteMemory() =>
            await WriteToMemory(utf8JsonPayloadWriterNoValidation);

        // OData JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf8_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf8);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf16_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf16);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf8Buffered_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf8Buffered);

        // Async OData JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf8Async_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf8Async);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf16Async_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf16Async);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataJsonWriterUtf8BufferdAsync_WriteMemory() =>
            await WriteToMemory(jsonWriterPayloadWriterUtf8BufferedAsync);

        // ODataMessageWriter-based writers

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf8_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf8);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf16_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf16);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf8NoValidation_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf8NoValidation);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf8Buffered_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf8Buffered);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterNoOp_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterNoOp);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterNoOpNoValodation_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterNoOpNoValidation);

        // Async ODataMessageWriter-based writers

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf8Async_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf8Async);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf16Async_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf16Async);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterUtf8NoValidationAsync_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterUtf8NoValidationAsync);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterNoOpAsync_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterNoOpAsync);

        [Benchmark]
        [BenchmarkCategory("InMemory")]
        public async Task ODataMessageWriterNoOpNoValidationAsync_WriteMemory() =>
            await WriteToMemory(odataWriterPayloadWriterNoOpNoValidationAsync);

        #endregion InMemory

        #region ToFile
        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task JsonSerializer_WriteToFile()
        {
            await JsonSerializer.SerializeAsync(memoryStream, data);
        }

        // Utf8JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task Utf8JsonWriter_WriteToFile() =>
            await WriteToFile(utf8JsonPayloadWriter);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task Utf8JsonWriterNoValidation_WriteToFile() =>
            await WriteToFile(utf8JsonPayloadWriterNoValidation);

        // OData JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf8_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf8);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf16_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf16);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf8Buffered_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf8Buffered);

        // Async OData JsonWriter-based writers

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf8Async_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf8Async);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf16Async_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf16Async);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataJsonWriterUtf8BufferdAsync_WriteToFile() =>
            await WriteToFile(jsonWriterPayloadWriterUtf8BufferedAsync);

        // ODataMessageWriter-based writers

        [Benchmark, BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf8_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf8);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf16_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf16);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf8NoValidation_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf8NoValidation);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf8Buffered_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf8Buffered);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterNoOp_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterNoOp);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterNoOpNoValodation_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterNoOpNoValidation);

        // Async ODataMessageWriter-based writers

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf8Async_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf8Async);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf16Async_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf16Async);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterUtf8NoValidationAsync_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterUtf8NoValidationAsync);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterNoOpAsync_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterNoOpAsync);

        [Benchmark]
        [BenchmarkCategory("ToFile")]
        public async Task ODataMessageWriterNoOpNoValidationAsync_WriteToFile() =>
            await WriteToFile(odataWriterPayloadWriterNoOpNoValidationAsync);
        #endregion ToFile

        [IterationSetup]
        public void SetupStreams()
        {
            memoryStream = new MemoryStream();
            string path = Path.GetTempFileName();
            fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        }

        public async Task WriteToMemory(IServerWriter<IEnumerable<Customer>> writer)
        {
            await writer.WritePayload(data, memoryStream);
        }

        [IterationCleanup]
        public void CleanUp()
        {
            fileStream.Close();
            fileStream = null;
        }

        public async Task WriteToFile(IServerWriter<IEnumerable<Customer>> writer)
        {
            await writer.WritePayload(data, fileStream);
        }
    }
}
