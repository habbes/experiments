using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalysis;

public class ArrayListDataCollection<T> : IDataCollection<T>
{
    private List<T> data;

    public ArrayListDataCollection(int capacity = 4)
    {
        this.data = new List<T>(capacity);
    }

    public void Add(T item)
    {
        this.data.Add(item);
    }

    public T GetItem(long index)
    {
        return this.data[(int)index];
    }

    public long GetLength()
    {
        return this.data.Count;
    }
}
