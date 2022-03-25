using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestServers(markModelImmutable: true);
        }

        static async Task TestServers(bool markModelImmutable)
        {
            IEdmModel model = DataModel.GetEdmModel();
            if (markModelImmutable)
            {
                model.MarkAsImmutable();
            }

            var data = CustomerDataSet.GetCustomers(1000);

            ServerCollection<IEnumerable<Customer>> servers = new(data, 8080);

            servers.AddServers(
                ("Baseline JSON", "utf-8", new JsonSerializerServerWriter()),
                ("Utf8JsonWriter Baseline", "utf-8", new Utf8JsonWriterBasicServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter","utf-8", new Utf8JsonWriterServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter-NoValidation", "utf-8", new Utf8JsonWriterServerWriter(stream =>
                    new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }))),
                ("OData UTF8 JsonWriter", "utf-8", new ODataJsonWriterServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriter())),
                ("OData UTF16 JsonWriter","utf-16", new ODataJsonWriterServerWriter(stream => stream.CreateUtf16ODataJsonWriter())),
                ("OData Async UTF8 JsonWriter", "utf-8", new ODataAsyncJsonWriterServerWriter(
                    stream => stream.CreateUtf8ODataJsonWriterAsync())),
                ("OData Async UTF16 JsonWriter", "utf-16", new ODataAsyncJsonWriterServerWriter(
                    stream => stream.CreateUtf16ODataJsonWriterAsync())),
                ("No-op JsonWriter", "utf-8", new ODataJsonWriterServerWriter(
                    stream => new NoopJsonWriter())),
                
                ("No-op Async JsonWriter", "utf-8", new ODataAsyncJsonWriterServerWriter(stream => new NoopJsonWriter())),
                ("OData Sync UTF-8", "utf-8", new ODataServerWriter(model, stream => stream.CreateUtf8Message())),
                ("OData Sync UTF-16", "utf-16",new ODataServerWriter(model, stream => stream.CreateUtf16Message())),
                ("OData Async UTF-8", "utf-8", new ODataAsyncServerWriter(model, stream => stream.CreateUtf8Message())),
                ("OData Async UTF-16", "utf-16", new ODataAsyncServerWriter(model, stream => stream.CreateUtf16Message())));

            servers.StartServers();
           
            Console.WriteLine("Press any key to terminate servers");
            Console.ReadKey();

            Console.WriteLine("Terminating servers...");
            await servers.StopServers();
            Console.WriteLine("Servers terminated");
        }
    }
}
