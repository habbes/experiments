using SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public class DictionaryDataCollection<T> : IDataCollection<T>
{
    Dictionary<long, T> _data = new Dictionary<long, T>();
    long _currentIndex = 0;
    public void Add(T item)
    {
        _data[_currentIndex] = item;
        _currentIndex++;
    }

    public T GetItem(long index)
    {
        if (_data.TryGetValue(index, out var item))
        {
            return item;
        }

        throw new ArgumentOutOfRangeException();
    }

    public long GetLength()
    {
        return _data.Count;
    }
}
