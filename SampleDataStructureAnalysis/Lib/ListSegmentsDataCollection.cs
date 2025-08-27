using SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib;

public class ListSegmentsDataCollection<T> : IDataCollection<T>
{
    List<T[]> segments;
    const int SegmentSize = 1000;
    long count = 0;

    public ListSegmentsDataCollection(int numSegments = 1000)
    {
        segments = new List<T[]>(capacity: numSegments);
    }

    public void Add(T item)
    {
        T[] segment;
        if (count == Capacity)
        {
            segment = new T[SegmentSize];
            this.segments.Add(segment);
        }
        else
        {
            // 2045
            // 2045
            int segmentIndex = (int)((count) / SegmentSize);
            segment = this.segments[segmentIndex];
        }

        int itemIndex = (int)((count) % SegmentSize);
        segment[itemIndex] = item;


        count++;
    }

    public T GetItem(long index)
    {
        if (index >= count)
        {
            throw new IndexOutOfRangeException();
        }

        int segmentIndex = (int)((index) / SegmentSize);
        var segment = this.segments[segmentIndex];

        int itemIndex = (int)((index) % SegmentSize);
        return segment[itemIndex];
    }

    public long GetLength()
    {
        return count;
    }

    public long Capacity => segments.Count * SegmentSize;
}
