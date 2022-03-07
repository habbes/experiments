using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    class Utf8JsonWriterSeverWriter : IServerWriter<IEnumerable<Customer>>
    {
        Func<Stream, Utf8JsonWriter> _writerFactory;

        public Utf8JsonWriterSeverWriter(Func<Stream, Utf8JsonWriter> writerFactory)
        {
            _writerFactory = writerFactory;
        }

        public async Task WritePayload(IEnumerable<Customer> payload, Stream stream)
        {
            var sw = new Stopwatch();
            sw.Start();
            var settings = new ODataMessageWriterSettings();
            settings.ODataUri = new ODataUri
            {
                ServiceRoot = new Uri("https://services.odata.org/V4/OData/OData.svc/")
            };
            //if (_useArrayPool)
            //{
            //    settings.ArrayPool = CharArrayPool.Shared;
            //}
            //InMemoryMessage message = new InMemoryMessage { Stream = stream };

            var jsonWriter = _writerFactory(stream);

            //var messageWriter = new ODataMessageWriter((IODataResponseMessage)message, settings, model);
            //var entitySet = model.EntityContainer.FindEntitySet("Customers");
            //var writer = await messageWriter.CreateODataResourceSetWriterAsync(entitySet);

            var resourceSet = new ODataResourceSet();
            //Console.WriteLine("Start writing resource set");
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("@odata.context", $"{settings.ODataUri.ServiceRoot.AbsoluteUri}/$metadata/#Customers");
            jsonWriter.WriteStartArray("value");



            //await writer.WriteStartAsync(resourceSet);
            //Console.WriteLine("About to write resources {0}", payload.Count());

            foreach (var customer in payload)
            {
                // await resourceSerializer.WriteObjectInlineAsync(item, elementType, writer, writeContext);
                // create resource with only primitive types
                var resource = new ODataResource
                {
                    Properties = new[]
                    {
                        new ODataProperty
                        {
                            Name = "Id",
                            Value = customer.Id
                        },
                        new ODataProperty { Name = "Name", Value = customer.Name },
                        new ODataProperty
                        {
                            Name = "Emails",
                            Value = new ODataCollectionValue
                            {
                                Items = customer.Emails,
                                TypeName = "Collection(Edm.String)"
                            }
                        }
                    }
                };

                //Console.WriteLine("Start writing resource {0}", customer.Id);
                //await writer.WriteStartAsync(resource);
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("Id", customer.Id);
                jsonWriter.WriteString("Name", customer.Name);
                jsonWriter.WriteStartArray("Emails");
                foreach (var email in customer.Emails)
                {
                    jsonWriter.WriteStringValue(email);
                }
                jsonWriter.WriteEndArray();

                // skip WriterStreamPropertiesAsync
                // WriteComplexPropertiesAsync
                // -- HomeAddress
                var homeAddressInfo = new ODataNestedResourceInfo
                {
                    Name = "HomeAddress",
                    IsCollection = false
                };
                // start write homeAddress
                //await writer.WriteStartAsync(homeAddressInfo);

                var homeAddressResource = new ODataResource
                {
                    Properties = new[]
                    {
                        new ODataProperty { Name = "City", Value = customer.HomeAddress.City },
                        new ODataProperty { Name = "Street", Value = customer.HomeAddress.Street }
                    }
                };
                //await writer.WriteStartAsync(homeAddressResource);
                //await writer.WriteEndAsync();
                jsonWriter.WriteStartObject("HomeAddress");
                jsonWriter.WriteString("City", customer.HomeAddress.City);
                jsonWriter.WriteString("Street", customer.HomeAddress.Street);

                // end write homeAddress
                //await writer.WriteEndAsync();
                jsonWriter.WriteEndObject();
                // -- End HomeAddress

                // -- Addresses
                var addressesInfo = new ODataNestedResourceInfo
                {
                    Name = "Addresses",
                    IsCollection = true
                };
                // start addressesInfo
                //await writer.WriteStartAsync(addressesInfo);
                jsonWriter.WriteStartArray("Addresses");


                var addressesResourceSet = new ODataResourceSet();
                // start addressesResourceSet
                //await writer.WriteStartAsync(addressesResourceSet);
                foreach (var address in customer.Addresses)
                {
                    var addressResource = new ODataResource
                    {
                        Properties = new[]
                        {
                            new ODataProperty { Name = "City", Value = address.City },
                            new ODataProperty { Name = "Street", Value = address.Street }
                        }
                    };

                    //await writer.WriteStartAsync(addressResource);
                    jsonWriter.WriteStartObject();
                    jsonWriter.WriteString("City", address.City);
                    jsonWriter.WriteString("Street", address.Street);
                    //await writer.WriteEndAsync();
                    jsonWriter.WriteEndObject();
                }

                // end addressesResourceSet
                //await writer.WriteEndAsync();
                jsonWriter.WriteEndArray();


                // end addressesInfo
                //await writer.WriteEndAsync();

                // -- End Addresses

                // end write resource
                //await writer.WriteEndAsync();
                jsonWriter.WriteEndObject();
                //Console.WriteLine("Finish writing resource {0}", customer.Id);
                //Console.WriteLine("Finised customer {0}", customer.Id);
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();
            await jsonWriter.FlushAsync();
        }
    }
}
