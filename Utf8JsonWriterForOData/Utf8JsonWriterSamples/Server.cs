using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    public class Server<T> : IDisposable
    {
        IServerWriter<T> _writer;
        T _data;
        HttpListener _server;
        bool _running = false;
        Task _runningTask;
        public Server(int port, IServerWriter<T> responseWriter, T data)
        {
            _writer = responseWriter;
            _data = data;
            _server = new HttpListener();
            _server.Prefixes.Add($"http://localhost:{port}/");
        }

        public void Dispose()
        {
            Stop().Wait();
        }

        public void Start()
        {
            if (_running)
            {
                return;
            }

            _running = true;
            _server.Start();
            _runningTask = HandleRequests();
        }

        public async Task Stop()
        {
            if (!_running)
            {
                return;
            }

            _running = false;
            _server.Stop();
            await _runningTask;
        }

        private async Task HandleRequests()
        {
            while (_running)
            {
                try
                {
                    HttpListenerContext ctx = await _server.GetContextAsync();
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;
                    Console.WriteLine("Request received: {0}", req.Url.ToString());

                    T responseData = default(T);

                    resp.ContentType = "application/json";

                    // allow use to specify data size with ?count=<count> query param
                    //if (!string.IsNullOrEmpty(req.Url.Query))
                    //{
                    //    var query = HttpUtility.ParseQueryString(req.Url.Query);
                    //    string rawCount = query.Get("count");
                    //    if (!string.IsNullOrEmpty(rawCount))
                    //    {
                    //        int count = int.Parse(rawCount);
                    //        responseData = DataSet.GetCustomers(count);
                    //    }
                    //}

                    responseData ??= _data;
                    var sw = new Stopwatch();
                    sw.Start();
                    await _writer.WritePayload(responseData, resp.OutputStream);
                    resp.Close();
                    sw.Stop();
                    Console.WriteLine("Response time {0}ms, request {1}", sw.ElapsedMilliseconds, req.Url.ToString());
                }
                catch (Exception) { }
            }
        }
    }
}

