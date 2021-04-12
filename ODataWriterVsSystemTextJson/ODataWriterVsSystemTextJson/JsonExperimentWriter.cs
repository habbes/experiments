using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ODataWriterVsSystemTextJson
{
    class JsonExperimentWriter : IExperimentWriter
    {
        public async Task WriteCustomers(IEnumerable<Customer> payload, Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, payload);
        }
    }
}
