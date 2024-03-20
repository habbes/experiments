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

    protected IDataCollection<T> CreateCollection<T>()
    {
        return DataCollectionFactory.Create<T>(collectionType);
    }
}