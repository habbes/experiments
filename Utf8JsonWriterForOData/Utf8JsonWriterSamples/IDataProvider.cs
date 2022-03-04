using System.Collections.Generic;

namespace Utf8JsonWriterSamples
{
    public interface IDataProvider<T>
    {
        IEnumerable<T> GetData(int count);
    }
}
