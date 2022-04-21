using Microsoft.OData;
using Microsoft.OData.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentsLib
{
    public static class WriterHelpers
    {
        static DefaultJsonWriterFactory jsonWriterFactory = new();

        public static IJsonWriter CreateUtf8ODataJsonWriter(this Stream stream)
        {
            return jsonWriterFactory.CreateJsonWriter(
                new StreamWriter(stream, Encoding.UTF8), false);
        }

        public static IJsonWriter CreateUtf16ODataJsonWriter(this Stream stream)
        {
            return jsonWriterFactory.CreateJsonWriter(
                new StreamWriter(stream, Encoding.Unicode), false);
        }

        public static IJsonWriterAsync CreateUtf8ODataJsonWriterAsync(this Stream stream)
        {
            return jsonWriterFactory.CreateAsynchronousJsonWriter(
                new StreamWriter(stream, Encoding.UTF8), false);
        }

        public static IJsonWriterAsync CreateUtf16ODataJsonWriterAsync(this Stream stream)
        {
            return jsonWriterFactory.CreateAsynchronousJsonWriter(
                new StreamWriter(stream, Encoding.Unicode), false);
        }

        public static IODataResponseMessage CreateUtf8Message(this Stream stream)
        {
            InMemoryMessage message = new InMemoryMessage { Stream = stream };
            message.SetHeader("Content-Type", "application/json; charset=UTF-8");
            return message;
        }

        public static IODataResponseMessage CreateUtf16Message(this Stream stream)
        {
            InMemoryMessage message = new InMemoryMessage { Stream = stream };
            message.SetHeader("Content-Type", "application/json; charset=UTF-16");
            return message;
        }

        public static IODataResponseMessage CreateNoopMessage(this Stream stream)
        {
            NoopJsonWriterFactory factory = new();

            IContainerBuilder services = new ContainerBuilder();
            services.AddDefaultODataServices();
            services.AddService<IJsonWriterFactory>(ServiceLifetime.Singleton, _ => factory);
            services.AddService<IJsonWriterFactoryAsync>(ServiceLifetime.Singleton, _ => factory);

            InMemoryMessage message = new InMemoryMessage
            {
                Stream = stream,
                Container = services.BuildContainer()
            };

            return message;
        }
    }
}
