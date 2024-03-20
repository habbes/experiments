namespace SampleAnalysis;

public interface IDataCollection<T>
{
    public void Add(T item);
    public long GetLength();
    public T GetItem(long index);
}
