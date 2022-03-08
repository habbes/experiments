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
    /// <summary>
    /// Writes Customers collection payload using <see cref="Utf8JsonWriter"/> directly.
    /// </summary>
    class Utf8JsonWriterBasicServerWriter : IServerWriter<IEnumerable<Customer>>
    {
        Func<Stream, Utf8JsonWriter> _writerFactory;

        public Utf8JsonWriterBasicServerWriter(Func<Stream, Utf8JsonWriter> writerFactory)
        {
            _writerFactory = writerFactory;
        }

        public async Task WritePayload(IEnumerable<Customer> payload, Stream stream)
        {
            var sw = new Stopwatch();
            sw.Start();
            var serviceRoot = new Uri("https://services.odata.org/V4/OData/OData.svc/");

            var jsonWriter = _writerFactory(stream);

            var resourceSet = new ODataResourceSet();
            //Console.WriteLine("Start writing resource set");
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("@odata.context", $"{serviceRoot}/$metadata/#Customers");
            jsonWriter.WriteStartArray("value");

            foreach (var customer in payload)
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteNumber("Id", customer.Id);
                jsonWriter.WriteString("Name", customer.Name);
                jsonWriter.WriteStartArray("Emails");
                foreach (var email in customer.Emails)
                {
                    jsonWriter.WriteStringValue(email);
                }
                jsonWriter.WriteEndArray();


                // -- HomeAddress
                // start write homeAddress
                jsonWriter.WriteStartObject("HomeAddress");
                jsonWriter.WriteString("City", customer.HomeAddress.City);
                jsonWriter.WriteString("Street", customer.HomeAddress.Street);

                // end write homeAddress
                jsonWriter.WriteEndObject();
                // -- End HomeAddress

                // -- Addresses
                jsonWriter.WriteStartArray("Addresses");

                // start addressesResourceSet
                foreach (var address in customer.Addresses)
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WriteString("City", address.City);
                    jsonWriter.WriteString("Street", address.Street);
                    jsonWriter.WriteEndObject();
                }

                // end addressesResourceSet
                jsonWriter.WriteEndArray();
                // -- End Addresses

                // end write resource
                jsonWriter.WriteEndObject();

                
            }
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();
            await jsonWriter.FlushAsync();
        }
    }
}