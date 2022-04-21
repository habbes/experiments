using System.Collections.Generic;

namespace ExperimentsLib
{
    public interface IDataProvider<T>
    {
        IEnumerable<T> GetData(int count);
    }
}
