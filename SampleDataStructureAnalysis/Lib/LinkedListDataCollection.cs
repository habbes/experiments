using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalysis;

public class LinkedListDataCollection<T> : IDataCollection<T>
{
    LinkedList<T> data = new LinkedList<T>();
    public void Add(T item)
    {
        data.AddLast(item);
    }

    public T GetItem(long index)
    {
        long i = 0;
        var node = data.First;
        while (node != null)
        {
            if (i == index)
            {
                return node.Value;
            }

            node = node.Next;
            i++;
        }

        throw new ArgumentOutOfRangeException();
    }

    public long GetLength()
    {
        return data.Count;
    }
}
