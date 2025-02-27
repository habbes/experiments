One of the upcoming changes in .NET 10 has caught my eye. It relates to iterating over arrays that a referenced behind an interface. To explain why this is cool, let's start with some background context.
 
Let's say we have an int array and we want to compute the sum using the following loop:
 
 
```C#
int GetSum(int[] data)
{
   int sum = 0;
   foreach (int item in data)
   {
      sum += item;
   }
}
```

We could also use the following basic for loop
 
 
```C#
int GetSum(int[] data)
{
   int sum = 0;
   for (int i = 0; i < data.Length; i++)
   {
      sum += data[i];
   }
}
```
 
Which of these two loops is better from a performance standpoint?
 
Let's run a quick benchmark to find out:
 
Benchmark code: https://tinyurl.com/yzry3p6m
 
The performance is similar. Well this was a trick question, these two versions produce similar code.
 
When you use foreach with an array, the C# lowers to code similar to that of basic loop with an incrementing counter. You can use a tool like ILSpy or https://sharplab.io to inspect the lowered or decompiler version of some .NET code.
 
Here's the lowered/decompiled version as seen in sharplab:
And here's the assembly code from the JIT:
 
 
As you can see, the two versions are identical.
 
However, this is not the standard behaviour of the foreach keyword, this just an optimization that the compiler applies for arrays because arrays are special data structures that are treated in a special way by the compiler and the runtime.
 
Generally, the way foreach works is that it looks for a method called GetEnumerator() on the target object. This method is expected to return an enumerator. The enumerator is an object with a MoveNext() method and  Current object. The compiler will generator code to iteratively call MoveNext() and return the Current object until MoveNext() returns false. This is how the iterator pattern is implemented across .NET. All standard collections implement this patterns. There's a common interface that's used to unify this pattern called IEnumerable<T> which is used to expose fancy methods for transforming and processing data under the System.Linq namespace, e.g. collection.Filter(...).Select(...).GroupBy(...).OrderBy(...).Take(...).ToList()
 
Anyway, I'm digressing. Let's use List<T> as an example to see how the compiler handles the general case:
 
```C#
public int IntSumWithListForeach(List<int> data)
{
    int sum = 0;
    foreach (int item in data)
    {
        sum += item;
    }
    
    return sum;
}
```

Here's the "lowered" version of the code:


Sharplab URL: https://tinyurl.com/59myf3t3
 
You can see the code generated for the List<T> enumeration follows the pattern explained above. This code does more work than a for loop. You can see the generated code looks more complex, has a lot more move instructions, etc.
 
 
Now, before we circle back to the array, there's another scenario that's relevant. In this case are accessing the List<T> through the concrete reference. But it's very common to have methods that accept an interface such as IList<T> or IEnumerable<T> where the concrete type could be any class that implements the interface.
 
Here we define the same method, but use IEnumerable<T> as the parameter type instead of List<T>:
 
 
```C#
public int IntSumWithIEnumerableForeach(IEnumerable<int> data)
{
    int sum = 0;
    foreach (int item in data)
    {
        sum += item;
    }
    
    return sum;
}
``` 
 
As you can see, the code we write is entirely the same, the only material difference is the change from List<int> to IEnumerable<int>. Since List<int> implements IEnumerable<T>, we can still pass our list to the same method and it will return the same result. 
 
Here's what the "lowered" version of the code looks like:
 
It looks pretty similar to what we had before, right? Well, not so fast, there's a crucial difference:
 
If you look closely you'll notice that in the array List<int> version, the enumerator object is defined as
 List<int>.Enumerator enumerator = data.GetEnumerator() whereas in the new version the enumerator is defined as IEnumerator<int>.Enumerator enumerator = data.GetEnumerator(). Well this subtle difference has considerable ramifications for performance:
 
First thing to bear in mind that if we pass the same list to both methods, the same data.GetEnumerator() method will end up being called at the end of the day. But how it gets called is the difference.
 
The List<T>.GetEnumerator() method returns a new instance of List<T>.Enumerator. This type is a struct and not a class. This means it's value type, and when it's returned from a function, it's stored on the stack without doesn't get allocated to the heap. Also List<T>.GetEnumerator() is not a virtual function, this means that if you're calling the method directly through a List<T> reference, the compiler knows at compile-time which method is going to get called, i.e. it can't be overridden by some child class that inherits from List<T>. List<T>.Enumerator methods and properties are also non-virtual. When the compiler knows exactly that the same method is going to be called regardless of the object type, it can perform some optimizatons line inlining, etc.
 
Now when the list.GetEnumerator() is called through an IEnumerable<T> interface, it calls this version of the method where the signature returns an IEnumerator<T>.  IEnumerator<T> is an interface, but the method returns the List<T>.Enumerator struct. This means the struct object will be cast to an IEnumerator<T> interface. When you cast a struct to an interface, it must become a reference type, and all reference types live on the heap. So an object will be allocated on the heap and the struct data will be copied into it. This process is called boxing. In performance-sensitive code paths, boxing is cost we would like to avoid.
 
In addition to boxing, now the enumerator is an interface, all its methods are virtual. This means when the compiler looks at enumerator.MoveNext() it doesn't know what the implementation of MoveNext is at compile-time. It can only find out at runtime, by looking at the object's method table to determine the correct implementation that should be called. This means that it cannot safely inline that method since the implementation could change from one call to the next in theory. But if the JIT compiler determines at runtime that this method gets called frequently with the same object type, it could create a path with optimized machine code for that version, almost treating it as in it was a non-virtual method call. This process is called devirtualization.
 
Now before we moving forward, let's look at add lists to our benchmarks:
 
 
Here's the code: https://tinyurl.com/yzry3p6m
 
Here we see the using different results when dealing with a list. Using a basic for loop is the fastest approach, only 1.5x slower than uing an array. Where does this cost come from? I haven't invesigated it, but my guess would be that fact that when you index into a List, it has to check whether the index is not past the Count of the list. There's also some layers of indirection because you access the list, then call the getter method of the indexer which finally access the internal array.
 
The foreach version is 1.9x slower than the array version. This is likely due to the added overhead of methods from MoveNext() and Current.
 
Finally, we use the foreach version using the IEnumerable is more than 2x slower than the other list versions. It also registers an allocation of 40B, this is the boxed enumerator.
 
As you can see iterating through the interface is slower than the concrete types.
 
Now back to the beginning, what would happen if we pass an array to the method accepting IEnumerable<int>. Well arrays happen to implement IEnumerable<T>. If you pass an array to variable of type IEnumerable<T> and call foreach, that call will be turned to the equivalent of IEnumerator<int> enumerator = (IEnumerable<int>)array).GetEnumerator(). Internally this calls some helper method which returns instance of an internal class called SZGenericArrayEnumerator<T>. So in the case of the array, the enumerator is a class, not a struct. Why is this the case? My guess is: there's no benefit of making it struct, when iterate over an concrete array, we don't need to an enumerator because the compiler rewrites it into a basic for loop. We only need the enumerator when we're behind an interface, in which case we'll allocate to the heap anyway since interfaces are reference types. So if we are going to allocate to the heap anyway, the we might as well skip the additional overhead of boxing. We'll still pay the overhead of virtual calls through IEnumerator<int>.MoveNext().
 
So let's look at benchmarks the scenario of iterating over an array behind an interface reference:
 
Benchmark code: https://tinyurl.com/3rp5tdy9
 
We see that the it's over 4x slower than using a basic for loop, and there's heap allocation. At least it's slightly faster than iterating over the list.
 
Hopefully now it's clearer why using foreach against an array behind an IEnumerable<T> interface is slower than a basic for loop. And this finally brings us to the point of this post, what is .NET 10 going to do about it? The .NET team is working on de-virtualizing array enumerations behind an interface. That means, when it knows that the IEnumerable<T> is actually a T[] array, the JIT will attempt to devirtualize the enumerator constructor an inline it, and do the same for the MoveNext() and Current call, this will remove the overhead of the virtual call (i.e. going through the method table to find the reference of the correct method to call) and inline removes the overhead of a method call altogether. In fact it may even optimize away the heap allocation of the enumerator. If it's sure it's not going to be used outside the method, it may allocate that enumerator on the stack and save the heap allocation. All this combined improves the performance of the loop.
 
However, right it does not seem to do this optimization in all scenarios. I've run some benchmarks and found that it will optimize the following scenario where it's 100% the IEnumerable<T> reference always point to an array:
 
 
```C#
private int IntSumWithForeach_ArrayCast(int[] arrayData) {
    int sum = 0;
    IEnumerable<int> data = arrayData;
    foreach (int item in data)
    {
        sum += item;
    }
    
    return sum;
}
```
 
 
However it does not seem to optimize the following scenario where the parameter is IEnumerable<T> even if it's called why an array. In this case there's a chance it could be another type. But if it's called frequently with an array, it should ideally detect this using Profile Guided Optimization and create an optimized path for that scenario, but I don't think that has been implemented yet, not 100% sure if it's on the roadmap but it sounds like it could.
 
Here's a new benchmark, now running in .NET 10, including the versions above with array cast to IEnumerable<T>.
 
 
I ran these benchmarks locally since Sharpbench doesn't support .NET 10 yet (contributions welcome by the way). That's why the numbers are very different from the earlier benchmark results. The code is here: https://github.com/habbes/experiments/tree/master/ArrayDevirtInDotNet10
 
But if you look at the last row, you can see that it's now only 1.67x slower than the basic for loop, all the way down from about 4x+. It also has no allocation despite using reference types. I didn't even know that this kind of optimization happens in .NET 10, it requires escape analysis which you'd mostly hear about in languages like Java or Go where you have less control of where your objects are allocated. But apparently it started in .NET 9. Pretty cool. But it's apparently very limited: https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-9/#object-stack-allocation
 
For reference, here's how the last columns look like in .NET 8 (I assume .NET 9 would be similar, but haven't tested)
 
 
As you can see the last row is still 4x slower than the base line and still allocates. The devirtualization doesn't get applied here.
 
Benchmark code: https://tinyurl.com/584zrsun
 
 
If you're interested in learning more about these optimization efforts in .NET, including the code that makes it happen, check out these resources:
.NET 10 Preview 1 announcement: https://devblogs.microsoft.com/dotnet/dotnet-10-preview-1/
Overview of the work: https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview1/runtime.md#array-interface-method-devirtualization
Issue that explains and tracks the various De-abstraction efforts in .NET 10, it explains the issue and fix in more details and contains links to even more details and links to pull requests: https://github.com/dotnet/runtime/issues/108913
They mention that if this works well for arrays, they'll consider bringing these optimizations to List<T>, and I'm really excited about that.