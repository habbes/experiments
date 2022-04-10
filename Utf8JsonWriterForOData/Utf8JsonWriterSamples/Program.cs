using ExperimentsLib;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExperimentsLib
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestServers(50);
        }

        static async Task TestServers(int dataSize = 5000)
        {
            var data = CustomerDataSet.GetCustomers(dataSize);

            ServerCollection<IEnumerable<Customer>> servers = DefaultServerCollection.Create(data);
            servers.StartServers(startPort: 8080);
           
            Console.WriteLine("Press any key to terminate servers");
            Console.ReadKey();

            Console.WriteLine("Terminating servers...");
            await servers.StopServers();
            Console.WriteLine("Servers terminated");
        }
    }
}
