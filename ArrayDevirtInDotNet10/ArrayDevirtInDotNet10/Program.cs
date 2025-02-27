using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

BenchmarkDotNet.Running.BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args);

[MemoryDiagnoser]
// [SimpleJob(RuntimeMoniker.Net80)]
// [SimpleJob(RuntimeMoniker.Net10_0)] // This doesn't seem to be available yet
public class Benchmarks
{

    public int dataSize = 1000;
    int[] array;
    List<int> list;

    public Benchmarks()
    {
        array = new int[dataSize];
        list = new List<int>(dataSize);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i;
            list.Add(i);
        }
    }

    [Benchmark(Baseline = true)]
    public int RunIntSumWithFor() => IntSumWithFor(array);

    [Benchmark]
    public int RunIntSumWithForeach() => IntSumWithForeach(array);

    [Benchmark]
    public int RunIntSumWithFor_List() => IntSumWithFor_List(list);

    [Benchmark]
    public int RunIntSumWithForeach_List() => IntSumWithForeach_List(list);

    [Benchmark]
    public int RunIntSumWithForeachIEnumerable_List() => IntSumWithForeachIEnumerable_List(list);

    [Benchmark]
    public int RunIntSumWithForeachIEnumerable_Array() => IntSumWithForeachIEnumerable_List(array);

    [Benchmark]
    public int RunIntSumWithForeach_ArrayCast() => IntSumWithForeach_ArrayCast(array);

    private int IntSumWithFor(int[] data) {
        int sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }
        
        return sum;
    }
    

    private int IntSumWithForeach(int[] data) {
        int sum = 0;
        foreach (int item in data)
        {
            sum += item;
        }
        
        return sum;
    }

    private int IntSumWithFor_List(List<int> data) {
        int sum = 0;
        for (int i = 0; i < data.Count; i++)
        {
            sum += data[i];
        }
        
        return sum;
    }
    

    private int IntSumWithForeach_List(List<int> data) {
        int sum = 0;
        foreach (int item in data)
        {
            sum += item;
        }
        
        return sum;
    }

    private int IntSumWithForeachIEnumerable_List(IEnumerable<int> data) {
        int sum = 0;
        foreach (int item in data)
        {
            sum += item;
        }
        
        return sum;
    }

    private int IntSumWithForeach_ArrayCast(int[] arrayData) {
        int sum = 0;
        IEnumerable<int> data = arrayData;
        foreach (int item in data)
        {
            sum += item;
        }
        
        return sum;
    }


}
