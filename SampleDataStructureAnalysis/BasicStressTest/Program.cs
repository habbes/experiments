using SampleAnalysis;
using System.Diagnostics;

string collectionType = args.Length > 1 ? args[1] : DataCollectionFactory.ArrayListCollectionName;
IDataCollection<int> data = DataCollectionFactory.Create<int>(collectionType);

Console.WriteLine($"Starting stress test with {collectionType}...{args[0]}");

long count = 0;
Stopwatch sw = Stopwatch.StartNew();
try
{
    while (true)
    {
        data.Add(10);
        count++;
        if (count % 1000 == 0)
        {
            Console.WriteLine($"Added {count} items to collection so far...");
        }
    }

}
catch (Exception ex)
{
    sw.Stop();
    Console.WriteLine();
    Console.WriteLine($"Program crashed. Successfully added {count} items before crash in {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}