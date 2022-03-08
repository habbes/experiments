using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    /// <summary>
    /// Manages a collection of servers that can be started and stopped
    /// at the same time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServerCollection<T>
    {
        T data;
        List<(string name, int port, Server<T> server)> servers = new();
        int nextPort;

        public ServerCollection(T data, int startPort = 8080)
        {
            this.data = data;
            nextPort = startPort;
        }

        public void AddServer(string name, IServerWriter<T> serverWriter)
        {
            servers.Add((name, nextPort, new Server<T>(nextPort, serverWriter, data)));
            nextPort++;
        }

        public void AddServers(params (string name, IServerWriter<T> serverWriter)[] servers)
        {
            foreach (var (name, serverWriter) in servers)
            {
                AddServer(name, serverWriter);
            }
        }

        public void StartServers()
        {
            foreach (var (name, port, server) in servers)
            {
                server.Start();
                Console.WriteLine($"{name} server running on http://localhost:{port}");
            }
        }

        public async Task StopServers()
        {
            await Task.WhenAll(servers.Select(item => item.server.Stop()));
        }

    }
}
