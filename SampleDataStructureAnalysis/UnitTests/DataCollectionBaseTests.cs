using SampleAnalysis;

namespace SampleAnalysis.Tests;

public abstract class DataCollectionBaseTests
{
    private string collectionType;

    public DataCollectionBaseTests(string collectionType)
    {
        this.collectionType = collectionType;
    }
    
    [Fact]
    public void AddAndRetrieveItems()
    {
        var collection = DataCollectionFactory.Create<int>(collectionType);

        collection.Add(10);
        collection.Add(20);
        collection.Add(30);

        Assert.Equal(10, collection.GetItem(0));
        Assert.Equal(20, collection.GetItem(1));
        Assert.Equal(30, collection.GetItem(2));
        Assert.Equal(3, collection.GetLength());
    }

    [Fact]
    public void AddAndRetrieveManyItems()
    {
        var collection = DataCollectionFactory.Create<int>(collectionType);

        int size = 1000_000;

        for (int i = 0; i < size; i++)
        {
            collection.Add(i);
        }

        for (int i = 0; i < size; i++)
        {
            Assert.Equal(i, collection.GetItem(i));
        }

        Assert.Equal(size, collection.GetLength());
    }

    protected IDataCollection<T> CreateCollection<T>()
    {
        return DataCollectionFactory.Create<T>(collectionType);
    }
}