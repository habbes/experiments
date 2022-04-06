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
        List<(string name, Server<T> server)> servers = new();
        int nextPort;

        public ServerCollection(T data)
        {
            this.data = data;
        }

        public void AddServer(string name, string charset, IServerWriter<T> serverWriter)
        {
            servers.Add((name, new Server<T>(serverWriter, data, charset: charset)));
            nextPort++;
        }

        public void AddServers(params (string name, string charset, IServerWriter<T> serverWriter)[] servers)
        {
            foreach (var (name, charset, serverWriter) in servers)
            {
                AddServer(name, charset, serverWriter);
            }
        }

        public void StartServers(int startPort = 8080)
        {
            int port = startPort;
            foreach (var (name, server) in servers)
            {
                server.Start(port);
                Console.WriteLine($"{name} server running on http://localhost:{port}");
                port++;
            }
        }

        public Server<T> StartServer(string name, int port)
        {
            Server<T> server = servers
                .Where(s => s.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.server)
                .FirstOrDefault();

            if (server == null)
            {
                throw new Exception($"Server not found '{name}'");
            }

            server.Start(port);
            return server;
        }

        public async Task StopServers()
        {
            await Task.WhenAll(servers.Select(item => item.server.Stop()));
        }

    }
}
