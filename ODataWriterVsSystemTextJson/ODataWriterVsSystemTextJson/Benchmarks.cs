using BenchmarkDotNet.Attributes;
using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ODataWriterVsSystemTextJson
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        IExperimentWriter jsonWriter;
        IExperimentWriter odataWriter;
        IExperimentWriter odataSyncWriter;
        IEnumerable<Customer> data;
        HttpClient httpClient;
        ExperimentServer jsonServer;
        ExperimentServer odataServer;
        ExperimentServer odataSyncServer;

        [Params(1000, 5000, 10000)]
        //[Params(5000)]
        public int dataSize;

        public Benchmarks()
        {
            jsonWriter = new JsonExperimentWriter();
            odataWriter = new ODataExperimentWriter(DataModel.GetEdmModel());
            odataSyncWriter = new ODataSyncExperimentWriter(DataModel.GetEdmModel());
            httpClient = new HttpClient();
        }

        [GlobalSetup]
        public void PrepareDataset()
        {
            data = DataSet.GetCustomers(dataSize);
        }

        #region Write to memory
        [Benchmark(Baseline = true)]
        public void WriteJson()
        {
            var stream = new MemoryStream();
            jsonWriter.WriteCustomers(data, stream).Wait();
        }

        [Benchmark]
        public void WriteOData()
        {
            var stream = new MemoryStream();
            odataWriter.WriteCustomers(data, stream).Wait();
        }

        [Benchmark]
        public void WriteODataSync()
        {
            var stream = new MemoryStream();
            odataSyncWriter.WriteCustomers(data, stream).Wait();
        }

        #endregion

        //#region Write to buffered memory
        //[Benchmark]
        //public void WriteJsonBufferedMemory()
        //{
        //    var pipe = new Pipe();
        //    var writer = pipe.Writer;
        //    var stream = writer.AsStream();
        //    jsonWriter.WriteCustomers(data, stream).Wait();

        //}

        //[Benchmark]
        //public void WriteODataBufferedMemory()
        //{
        //    var pipe = new Pipe();
        //    var writer = pipe.Writer;
        //    var stream = writer.AsStream();
        //    odataWriter.WriteCustomers(data, stream).Wait();

        //}

        //[Benchmark]
        //public void WriteODataSyncBufferedMemory()
        //{
        //    var pipe = new Pipe();
        //    var writer = pipe.Writer;
        //    var stream = writer.AsStream();
        //    odataSyncWriter.WriteCustomers(data, stream).Wait();
        //}

        //#endregion

        //#region Write to local file

        string tempFile;
        Stream fileStream;


        [IterationSetup(Targets = new[]{ nameof(WriteJsonFile), nameof(WriteODataFile), nameof(WriteODataSyncFile)})]
        public void CreateTempFile()
        {
            tempFile = Path.GetTempFileName();
            fileStream = new FileStream(tempFile, FileMode.Create);
        }

        [IterationCleanup(Targets = new[] { nameof(WriteJsonFile), nameof(WriteODataFile), nameof(WriteODataSyncFile) })]
        public void CleanTempFile()
        {
            File.Delete(tempFile);
        }

        [Benchmark]
        public void WriteJsonFile()
        {
            WriteToFile(jsonWriter);
        }

        [Benchmark]
        public void WriteODataFile()
        {
            WriteToFile(odataWriter);
        }

        [Benchmark]
        public void WriteODataSyncFile()
        {
            WriteToFile(odataSyncWriter);
        }

        private void WriteToFile(IExperimentWriter writer)
        {
            writer.WriteCustomers(data, fileStream).Wait();
            fileStream.Close();
        }
        //#endregion

        #region Write to (local) network
        //[GlobalSetup(Target = nameof(WriteJsonHttp))]
        //public void SetupJsonServer()
        //{
        //    jsonServer = new ExperimentServer(9000, jsonWriter, data);
        //    jsonServer.Start();
        //}

        //[GlobalCleanup(Target = nameof(WriteJsonHttp))]
        //public void StopJsonServer()
        //{
        //    jsonServer.Stop().Wait();
        //}

        //[Benchmark]
        //public void WriteJsonHttp()
        //{
        //    MakeHttpRequest("http://localhost:9000").Wait();
        //}

        // TODO: the async writer hangs when writing the response to the http stream during the benchmark
        // not sure why this happens.
        //[GlobalSetup(Target = nameof(WriteODataHttp))]
        //public void SetupODataServer()
        //{
        //    odataServer = new ExperimentServer(9001, odataWriter, data);
        //    odataServer.Start();
        //}

        //[GlobalCleanup(Target = nameof(WriteODataHttp))]
        //public void StopODataServer()
        //{
        //    odataServer.Stop().Wait();
        //}

        //[Benchmark]
        //public void WriteODataHttp()
        //{
        //    MakeHttpRequest("http://localhost:9001/odata").Wait();
        //}

        //[GlobalSetup(Target = nameof(WriteODataSyncHttp))]
        //public void SetupODataSyncServer()
        //{
        //    odataSyncServer = new ExperimentServer(9002, odataWriter, data);
        //    odataSyncServer.Start();
        //}

        //[GlobalCleanup(Target = nameof(WriteODataSyncHttp))]
        //public void StopODataSyncServer()
        //{
        //    odataSyncServer.Stop().Wait();
        //}

        //[Benchmark]
        //public void WriteODataSyncHttp()
        //{
        //    MakeHttpRequest("http://localhost:9002/odata").Wait();
        //}

        private async Task MakeHttpRequest(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }

        #endregion
    }
}
