using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    /// <summary>
    /// Writes Customer collection payload using <see cref="JsonSerializer"/>.
    /// </summary>
    public class JsonSerializerServerWriter : IServerWriter<IEnumerable<Customer>>
    {
        public async Task WritePayload(IEnumerable<Customer> payload, Stream stream)
        {
            await JsonSerializer.SerializeAsync(stream, payload);
        }
    }
}
