# Sample Data Structure Analysis

This sample demonstrates how we can analyse various data structures in order to pick the most suitable one for a given problem and set of constraints.

## Problem and constraints

Our application receives a lot of data that needs to be processed in memory. This data is being pushed to our application in real time. We can assume it's numerical data (numbers).
We need an efficient way to add this data in memory as arrives. We want to keep as much of this data in memory as we can in order to allow for efficient in-memory processing.
What data structures can we use to achieve this?

Let's look at the operations we care about and the constraints more closely.

### Operations we need to support

- Adding new entries to the collection
- Fetching an entry by index
- Getting the total size of the collection
- Iterating through arbitrary segments of the collection for processing

Sample interface:

```csharp
interface IDataCollection<T>
{
    void AddItem(T item);
    T GetItem(long index);
    long GetLength();
}
```

### Considerations we need to make

- We may need to store billions of entry.
  - If we have 1B integers, how much memory will our data structure require to store?
  - If we have 8GB of available memory, how many integers can we fit in our data structure?

- We'll be adding new entries frequently?
  - How fast does it take to add a new entry?
  - How much memory do we need to add a new entry?

- We'll fetch data frequently for processing
  - How fast does it take to fetch a single item by index?
  - How fast can we iterate through sequence of items?

## Exploring different options

In this experiment we explore different data structures that can satisfy our functional requirements, then
we'll evaluate them analytical and quantitavely to pick the best one.

- Dynamic array
- Linked list
- Balanced Binary search tree that is sorted by insertion index

### Evaluating the differention options analytically

We'll analyse the Big-O runtime and space complexity of the different operations that we care about:

| Data Structure | Adding a new entry to the collection | Retrieving an item by index | Iterating through n items from a specified start point
|---------------|----------------------|--------|---------|
| Dynamic Array (Time) | O(1) (amortized) | T: O(1) | T: O(n) |
| Dynamic Array (Space)  | O(1) (amortized) | S: O(1) | S : O(1) |
| Linked List (Time) | O(1) | O(n) | O(n)
| Linked List (Space)             | O(1) | O(n) | O(n)
| Balanced BST (Time) | O(logn) | O(logn) | O(n) (see explanation below)
| Balanced BST (Space) | O(1) | O(1) | O(h)

Regarding iterating through a BST in sorted order, if we have the minimum-value node (lowest index in the sequence), the next index will either be the parent of that node or the left most value in the parent's right subtree. We can find the next node in O(1) amortized (overall O(n) for n nodes), but we have to use a stack to keep track of values (which is where the O(h) space complexity comes from). [This article](https://mariazacharia-k.medium.com/binary-search-tree-bst-iterator-e01b7b933594) explains the algorithm in more detail. We can also do a naive iteration in O(1) space, by manually searching for the next smallest node in the parent's subtree, the time complexity would be O(nlogn) worst case in this scenario.

Beyond the Big-O complexity, here are some additional considerations:

- In dynamic arrays, the data items are stored sequentially in a contiguous block of memory. This makes them benefit from cache locality when iterating through items sequentially. Linked lists and trees do not have this property because data items maybe scattered in memory
- In a dynamic array, each entry only stores the data item. This makes it more memory efficient than a linked list also has to store the pointers to the next and previous items. Tree nodes also have to store points to the children and parents, and in our case also each node also has to store the item's index, which is what we use to store the tree.
- A dynamic array used a fixed-sized array to store data. When the array is full, the dynamic array grows by allocating a new array that's twice the size and copying items. If we have frequent resizes, it can get expensive. We can also run out of memory when adding a new item even if we technically have enough memory available for that extra item, simply because we need to allocate twice the size of the current array

### Evaluating the options using benchmarks

I created different implementations of `IDataCollection<T>`:

- [`ArrayListDataCollection<T>`](./Lib/ArrayListDataCollection.cs) based on [`List<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-8.0), the standard implementation of dynamic arrays in .NET
- [`LinkedListDataCollection<T>`](./Lib/LinkedListDataCollection.cs) based on [`LinkedList<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-8.0), an standard implementation of a doubly-linked list
- [`TreeDataCollection<T>`](./Lib/TreeDataCollection.cs) based on [`SortedDictionary<long, T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2?view=net-8.0), which is based on implementation of [red-black trees](https://en.wikipedia.org/wiki/Red%E2%80%93black_tree)

These implementations can be found in the [`Lib`](./Lib) directory.

Here are the results of running the benchmarks on my machine. Ideally, you would run the benchmarks
on environment that's similar to your production environment, i.e. same OS version, same .NET version, similar processor, etc.

To run the benchmarks, navigate to the [`Benchmarks`](./Benchmarks/) directory and run `dotnet run -c Release`

```sh
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100-preview.2.24078.1
  [Host]   : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  ShortRun : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX-512F+CD+BW+DQ+VL

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3
```

| Method               | dataSize | collectionType | Mean               | Error               | StdDev            | Gen0      | Gen1      | Allocated  |
|--------------------- |--------- |--------------- |-------------------:|--------------------:|------------------:|----------:|----------:|-----------:|
| CreateAndInsertItems | 1000     | arrayList      |       3,560.674 ns |       3,608.6212 ns |       197.8007 ns |         - |         - |    65536 B |
| GetItemByIndex       | 1000     | arrayList      |           1.146 ns |           1.0351 ns |         0.0567 ns |         - |         - |          - |
| CreateAndInsertItems | 1000     | linkedList     |     113,093.123 ns |      77,874.6413 ns |     4,268.5719 ns |    7.5684 |    3.6621 |    48000 B |
| GetItemByIndex       | 1000     | linkedList     |         582.177 ns |         270.4760 ns |        14.8257 ns |         - |         - |          - |
| CreateAndInsertItems | 1000     | sortedTree     |     361,954.867 ns |     469,887.4811 ns |    25,756.1184 ns |    8.7891 |    4.3945 |    56000 B |
| GetItemByIndex       | 1000     | sortedTree     |          24.624 ns |           0.9613 ns |         0.0527 ns |         - |         - |          - |
| CreateAndInsertItems | 1000000  | arrayList      |   3,604,378.125 ns |   3,355,139.1106 ns |   183,906.4961 ns |         - |         - | 67108866 B |
| GetItemByIndex       | 1000000  | arrayList      |           1.685 ns |          12.7855 ns |         0.7008 ns |         - |         - |          - |
| CreateAndInsertItems | 1000000  | linkedList     | 113,659,893.333 ns | 116,119,382.6957 ns | 6,364,895.1929 ns | 7600.0000 | 3800.0000 | 48000022 B |
| GetItemByIndex       | 1000000  | linkedList     |   2,373,432.422 ns |   2,136,665.0789 ns |   117,117.8228 ns |         - |         - |        2 B |
| CreateAndInsertItems | 1000000  | sortedTree     | 349,276,916.667 ns | 158,393,875.8087 ns | 8,682,102.8093 ns | 8500.0000 | 4000.0000 | 56000200 B |
| GetItemByIndex       | 1000000  | sortedTree     |          41.871 ns |          14.1720 ns |         0.7768 ns |         - |         - |          - |

The results shows that the `List<T>` implementation had better runtime across the board but allocated more memory when inserting items. The final size
of the `List<T>` based collection could occupy less memory than the linked list or tree, but because we had to resize the array list a couple of times
before the final size, all those may add up. But they are temporary and will be garbage collected if the runtime needs to free up memory.

We ran the benchmarks for 1000 and 1000,000 entries. In practice, it may not be necessary to do this because we already know the time complexity of these
operations so we can already predict how they will scale for different collection sizes. But I opted to include different data sizes for the sake of demonstration.
You can see that the data aligns with the complexity analysis from the previous section:

In all cases, the space allocated when inserting 1,000,000 entries is roughly 1,000 items the memory allocated when inserting 1,000 items. This makes
sense because storing n elements is O(n) for all the data structures. In the case of the array list, we see more memory allocated than with the other data structures.
This is likely due to the fact that the `List<T>` is resized (grows) multiple times, and each time a new array is allocated, these arrays add up. But the final
size of the `List<T>`-based likely takes up less space than the other data structures.

The array list takes up roughly the same amount of time (~1.1 and 1.6ms) to fetch the middle item by index regardless of whether the collection has 1,000 or 1,000,000 items.
This is expected because index-based access id O(1) for an array list.

The look-up by index is faster in a sorted tree than an array list. The sorted tree look-up scales pretty well in a sorted tree, from the 1000 to 1,000,000 entries (1000x increase),
the runtime only increases from 25 to 42ns (1.7x increase). However, the linked list scales poorly for this operation. Going from 582ns to 2,373,432ns (~4000x increase), the latter figure is much higher
than I would expect and would make a good subject for a follow-up investigation.

The decision between an linked list and sorted tree would be harder for this scenario because we have to make a trade-off between insertion time and retrieval time. But the array list is much faster across all operations compared to the other data structures (at least 22x faster on index-based retrieval and 31x faster on insertion).

### Evaluating the different options using basic stress test

We have a very simple stress test that basically tries to figure out how much data we can add to the collection before it runs out of memory. The test is a simple "infinite" while loop that adds an item to the collection until the program crashes (due to an `OutOfMemoryException`). The stress test source code is in the [`BasicStressTest`](./BasicStressTest/) directory.

We use docker to run the test so that we can easily restrict the memory limit available to the program.

To run the test, first build the docker image by runn the following comming in the root of this solution (where there's `Dockerfile` file):

`
docker build -t sample-analysis-stress-test -f Dockerfile .
`

This creates a docker image called `sample-analysis-stress-test`.

Now you can run a container based on this image by passing the implementation as an argument to the container. The implementation is either `arrayList`, `linkedList` and `sortedTree`.

For example, run the stress test in a container with a 512MB memory limit and use the `arrayList` implementation:

```sh
docker run -m 512MB -it sample-analysis-stress-test -- arrayList
```

The container periodically prints the progress of how many entries it has added so far. Once it crashes, it shows how many entries it was able to add.
The last lines of the output look something like:

```sh
Added 33549000 items to collection so far...
Added 33550000 items to collection so far...
Added 33551000 items to collection so far...
Added 33552000 items to collection so far...
Added 33553000 items to collection so far...
Added 33554000 items to collection so far...

Program crashed. Successfully added 33554432 items before crash in 982ms

Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at Program.<Main>$(String[] args) in /App/BasicStressTest/Program.cs:line 15
```

Here are the results, showing how many entries it was able to add to the collection with 512MB linit:

| Data Structure | Total Entries | Time till crash |
|----------------|---------------|---------------|
|Dynamic array | 33,444,432 | 982ms|
|Linked list | 8,267,118  | 4,629ms|
|Sorted tree | 7,087,458 | 7,911ms|

### Selecting a winner

Based on the results, the dynamic array seems to perform better than the other implementations. It's faster to add an retrieve items, and it can fit more data for the same available memory. If I were to pick an implement, among these 3, I would go with the dynamic array.

## Can we do better?

I'm still concerned about the resizing issue with dynamic arrays. We can reduce the cost of resizing the dynamic array by initializing the List<T> with a capacity that estimates how much data we'll need to add. Let's say we have a billion integers, which would occupy around 4GB of memory, and let's say our capacity is full. If we add one more item, it will need to allocate 8GB worth of memory. What if we only have 6GB available? Should this operation fail even if we only need to add a small number of entries?

Can we come up with an alternative? A data structure that would be more suited for this scenario than standard `List<T>`? Something that can fit a lot more data in the same space constraints, without sacrificing too much the insertion and index-based lookups. For example, something with the following properties:

- O(1) insertion at the end of the collection
- O(1) or O(logn) lookup by index
- O(n) iteration of an arbitrary sequence of the collection
- Can store more data than `List<T>` before crashing for the same memory limit

You can test your implementation against the other ones in this sample by:

- creating a class that implements the `IDataCollection<T>` interface in [`Lib`](./Lib).
- Then register your class in the [`DataCollectionFactory`](./Lib/DataCollectionFactory.cs) by giving it a name (e.g. `myDataStructure`)
- Add the name of your data structure to the params of the `collectionType` field in the [`DataCollectionBenchmarks`](./Benchmarks/DataCollectionBenchmarks.cs)
- Re-run the benchmarks and compare results
- Re-build the stress test docker image, run the container with your data structure name as argument and compare your results to the other
