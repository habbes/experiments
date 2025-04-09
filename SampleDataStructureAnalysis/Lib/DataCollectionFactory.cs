using Lib;

namespace SampleAnalysis;

public static class DataCollectionFactory
{
    public const string ArrayListCollectionName = "arrayList";
    public const string LinkedListCollectionName = "linkedList";
    public const string SortedTreeCollectionName = "sortedTree";
    public const string DictionaryCollectionName = "dict";
    public const string ArraySegmentsCollectionName = "arraySegments";
    public static IDataCollection<T> Create<T>(string name)
    {
        return name switch
        {
            ArrayListCollectionName => new ArrayListDataCollection<T>(),
            LinkedListCollectionName => new LinkedListDataCollection<T>(),
            SortedTreeCollectionName => new TreeDataCollection<T>(),
            DictionaryCollectionName => new DictionaryDataCollection<T>(),
            ArraySegmentsCollectionName => new ListSegmentsDataCollection<T>(),

            _ => throw new Exception($"Unknown data collection implementation {name}")
        };
    }
}
