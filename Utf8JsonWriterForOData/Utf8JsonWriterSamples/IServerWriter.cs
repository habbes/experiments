using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    public interface IServerWriter<T>
    {
        Task WritePayload(T payload, Stream stream);
    }
}
