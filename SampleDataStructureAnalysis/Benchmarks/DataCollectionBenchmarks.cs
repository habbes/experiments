using BenchmarkDotNet.Attributes;
using SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks;


[MemoryDiagnoser]
[ShortRunJob]
public class DataCollectionBenchmarks
{
    [Params(1000, 1000000)]
    public int dataSize;

    [Params(
        DataCollectionFactory.ArrayListCollectionName,
        DataCollectionFactory.LinkedListCollectionName,
        DataCollectionFactory.SortedTreeCollectionName)]
    public string collectionType;

    public IDataCollection<int> emptyCollection;
    public IDataCollection<int> fullCollection;

    [GlobalSetup]
    public void Setup()
    {
        emptyCollection = DataCollectionFactory.Create<int>(collectionType);
        fullCollection = DataCollectionFactory.Create<int>(collectionType);

        for (int i = 0; i < dataSize; i++)
        {
            fullCollection.Add(i);
        }
    }


    [Benchmark]
    public IDataCollection<int> CreateAndInsertItems()
    {
        for (int i = 0; i < dataSize; i++)
        {
            emptyCollection.Add(i);
        }

        return emptyCollection;
    }

    [Benchmark]
    public int GetItemByIndex()
    {
        int first = fullCollection.GetItem(0);
        int middle = fullCollection.GetItem(fullCollection.GetLength() / 2);
        int last = fullCollection.GetItem(fullCollection.GetLength() - 1);
        return first + middle + last;
    }
}
