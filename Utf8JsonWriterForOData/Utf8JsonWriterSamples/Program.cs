using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
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
                ("Baseline JSON", new JsonSerializerServerWriter()),
                ("Utf8JsonWriter Baseline", new Utf8JsonWriterBasicServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter", new Utf8JsonWriterServerWriter(stream => new Utf8JsonWriter(stream))),
                ("Utf8JsonWriter-NoValidation", new Utf8JsonWriterServerWriter(stream =>
                    new Utf8JsonWriter(stream, new JsonWriterOptions { SkipValidation = true }))),
                ("OData JsonWriter", new ODataJsonWriterServerWriter()),
                ("OData Sync", new ODataServerWriter(model)),
                ("OData Async", new ODataAsyncServerWriter(model)));

            servers.StartServers();
           
            Console.WriteLine("Press any key to terminate servers");
            Console.ReadKey();

            Console.WriteLine("Terminating servers...");
            await servers.StopServers();
            Console.WriteLine("Servers terminated");
        }
    }
}
