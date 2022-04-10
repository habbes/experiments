using ExperimentsLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using ExperimentsLib;

namespace SimpleServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error. Server name is required as the first argument.");
                PrintHelp();
                Environment.Exit(1);
            }

            string serverName = args[0];
            int port = 5000;
            int dataSize = 50;
            string host = "http://locahost";
            // rudimentary arg parser
            try
            {
                port = args.Where(a => a.StartsWith("--port="))
                    .Select(p => p.Split("=")[1])
                    .Select(int.Parse)
                    .FirstOrDefault();

                if (port == 0) port = 5000;

                dataSize = args.Where(a => a.StartsWith("--dataCount="))
                    .Select(p => p.Split("=")[1])
                    .Select(int.Parse)
                    .FirstOrDefault();
                if (dataSize == 0) dataSize = 50;

                host = args.Where(a => a.StartsWith("--host="))
                    .Select(p => p.Split("=")[1])
                    .FirstOrDefault();
                if (host == null) host = "http://localhost";
                if (!host.StartsWith("http")) host = $"http://{host}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse args {ex.Message}");
                PrintHelp();
                Environment.Exit(1);
            }

            var data = CustomerDataSet.GetCustomers(dataSize);
            var servers = DefaultServerCollection.Create(data, baseHost: host);

            var server = servers.StartServer(serverName, port);
            Console.WriteLine($"Using writer '{serverName}'. Response item count: {dataSize}.");
            Console.WriteLine($"Server running on port {port}");
            Console.WriteLine("Application started.");

            Console.WriteLine("Press any key to terminate server");
            Console.ReadKey();

            Console.WriteLine("Stopping server...");
            await server.Stop();
            Console.WriteLine("Server terminated");
        }

        static void PrintHelp()
        {
            Console.WriteLine("Run with arguments: <serverName> [--host=S] [--port=N] [--length=N]");
        }
    }
}
