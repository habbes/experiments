using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
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

            var data = CustomerDataSet.GetCustomers(100);
            var jsonServer = new Server<IEnumerable<Customer>>(8080, new Utf8JsonWriterSeverWriter(), data);
            jsonServer.Start();
            Console.WriteLine("JSON server running on http://localhost:8080");

            var odataServer = new Server<IEnumerable<Customer>>(8081, new ODataServerWriter(model), data);
            odataServer.Start();
            Console.WriteLine("OData server running on http://localhost:8081");

            var odataAsyncServer = new Server<IEnumerable<Customer>>(8082, new ODataAsyncServerWriter(model), data);
            odataAsyncServer.Start();
            Console.WriteLine("OData async server running on http://localhost:8082");

            Console.WriteLine("Press any key to terminate servers");
            Console.ReadKey();
            Console.WriteLine("Terminating servers...");
            await jsonServer.Stop();
            await odataServer.Stop();
            await odataAsyncServer.Stop();
            Console.WriteLine("Servers terminated");
        }
    }
}
