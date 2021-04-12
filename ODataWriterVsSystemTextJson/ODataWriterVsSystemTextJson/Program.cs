using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace ODataWriterVsSystemTextJson
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestWriters().Wait();

            //TestServers().Wait();

            RunBenchmarks();
        }

        static void RunBenchmarks()
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }

        static async Task TestServers()
        {
            var data = DataSet.GetCustomers(5000);
            var jsonServer = new ExperimentServer(8080, new JsonExperimentWriter(), data);
            jsonServer.Start();
            Console.WriteLine("JSON server running on http://localhost:8080");

            var odataServer = new ExperimentServer(8081, new ODataExperimentWriter(DataModel.GetEdmModel()), data);
            odataServer.Start();
            Console.WriteLine("OData server running on http://localhost:8081");

            var odataSyncServer = new ExperimentServer(8082, new ODataSyncExperimentWriter(DataModel.GetEdmModel()), data);
            odataSyncServer.Start();
            Console.WriteLine("OData sync server running on http://localhost:8082");

            Console.WriteLine("Press any key to terminate servers");
            Console.ReadKey();
            Console.WriteLine("Terminating servers...");
            await jsonServer.Stop();
            await odataServer.Stop();
            await odataSyncServer.Stop();
            Console.WriteLine("Servers terminated");
        }

        static async Task TestWriters()
        {
            var sw = new Stopwatch();
            var data = DataSet.GetCustomers(5000);
            var jsonWriter = new JsonExperimentWriter();
            var odataWriter = new ODataExperimentWriter(DataModel.GetEdmModel());
            var odataSyncWriter = new ODataSyncExperimentWriter(DataModel.GetEdmModel());

            Console.WriteLine("Writing JSON");
            var jsonStream = new MemoryStream();
            sw.Start();
            await jsonWriter.WriteCustomers(data, jsonStream);
            sw.Stop();
            jsonStream.Position = 0;
            //Console.WriteLine(new StreamReader(jsonStream).ReadToEnd());
            Console.WriteLine("JSON complete in {0}ms, press any key to continue...", sw.ElapsedMilliseconds);
            Console.ReadKey();

            Console.WriteLine("Writing OData");
            var odataStream = new MemoryStream();
            sw.Start();
            await odataWriter.WriteCustomers(data, odataStream);
            sw.Stop();
            //odataStream.Position = 0;
            //Console.WriteLine(new StreamReader(odataStream).ReadToEnd());
            Console.WriteLine("OData complete in {0}ms, press any key to continue...", sw.ElapsedMilliseconds);
            Console.ReadKey();

            Console.WriteLine("Writing OData synchronously");
            var odataSyncStream = new MemoryStream();
            sw.Start();
            await odataSyncWriter.WriteCustomers(data, odataSyncStream);
            sw.Stop();
            odataSyncStream.Position = 0;
            //Console.WriteLine(new StreamReader(odataStream).ReadToEnd());
            Console.WriteLine("OData sync complete in {0}ms, press any key to continue...", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
