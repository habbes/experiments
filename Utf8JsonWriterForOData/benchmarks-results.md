# Benchmark results analysis

## MicroBenchmarks

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.20348
Intel Xeon E-2336 CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=        5.0.404 [C:\Program Files\dotnet\sdk]
  [Host] : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT

Toolchain=InProcessEmitToolchain  InvocationCount=1  IterationCount=3
LaunchCount=1  UnrollFactor=1  WarmupCount=3

|     Method |                                         WriterName |        Mean |     Error |   StdDev |      Gen 0 |     Gen 1 |  Allocated |
------------ |--------------------------------------------------- |------------:|----------:|---------:|-----------:|----------:|-----------:|
 WriteToFile |                                     JsonSerializer |    31.94 ms |  3.143 ms | 0.172 ms |          - |         - |      68 KB |
 WriteToFile |                                         NoOpWriter |    23.39 ms |  8.909 ms | 0.488 ms | 11000.0000 |         - |  68,169 KB |
 WriteToFile |                                   NoOpWriter-Async |    35.70 ms |  1.253 ms | 0.069 ms | 11000.0000 |         - |  68,169 KB |
 WriteToFile |                             ODataJsonWriter-Direct |    36.98 ms |  1.375 ms | 0.075 ms |  1000.0000 |         - |   6,676 KB |
 WriteToFile |                       ODataJsonWriter-Direct-Async |   214.78 ms | 11.656 ms | 0.639 ms | 10000.0000 |         - |  61,373 KB |
 WriteToFile |          ODataJsonWriter-Direct-ResourceGeneration |    39.76 ms |  1.068 ms | 0.059 ms |  2000.0000 |         - |  15,660 KB |
 WriteToFile |    ODataJsonWriter-Direct-ResourceGeneration-Async |   208.24 ms | 15.778 ms | 0.865 ms |  8000.0000 |         - |  52,388 KB |
 WriteToFile |                              ODataJsonWriter-Utf16 |    71.21 ms |  2.636 ms | 0.144 ms | 12000.0000 |         - |  74,836 KB |
 WriteToFile |                        ODataJsonWriter-Utf16-Async |   295.82 ms | 17.975 ms | 0.985 ms | 24000.0000 |         - | 147,939 KB |
 WriteToFile |                               ODataJsonWriter-Utf8 |    65.11 ms |  2.332 ms | 0.128 ms | 12000.0000 |         - |  74,841 KB |
 WriteToFile |                         ODataJsonWriter-Utf8-Async |   287.19 ms | 16.567 ms | 0.908 ms | 24000.0000 |         - | 147,944 KB |
 WriteToFile |                      ODataJsonWriter-Utf8-Buffered |    60.11 ms |  1.821 ms | 0.100 ms | 12000.0000 | 1000.0000 |  75,248 KB |
 WriteToFile |                ODataJsonWriter-Utf8-Buffered-Async |   269.19 ms | 18.228 ms | 0.999 ms | 23000.0000 | 1000.0000 | 142,321 KB |
 WriteToFile |                            ODataMessageWriter-NoOp |   256.65 ms | 11.282 ms | 0.618 ms | 40000.0000 |         - | 248,025 KB |
 WriteToFile |                      ODataMessageWriter-NoOp-Async |   415.89 ms |  7.329 ms | 0.402 ms | 44000.0000 |         - | 272,260 KB |
 WriteToFile |               ODataMessageWriter-NoOp-NoValidation |   217.60 ms |  3.837 ms | 0.210 ms | 37000.0000 |         - | 226,930 KB |
 WriteToFile |         ODataMessageWriter-NoOp-NoValidation-Async |   374.49 ms | 22.461 ms | 1.231 ms | 41000.0000 |         - | 251,164 KB |
 WriteToFile |                           ODataMessageWriter-Utf16 |   337.19 ms |  7.072 ms | 0.388 ms | 41000.0000 |         - | 254,599 KB |
 WriteToFile |                     ODataMessageWriter-Utf16-Async | 1,042.25 ms | 88.374 ms | 4.844 ms | 70000.0000 | 5000.0000 | 429,623 KB |
 WriteToFile |                            ODataMessageWriter-Utf8 |   329.14 ms | 18.155 ms | 0.995 ms | 41000.0000 |         - | 254,604 KB |
 WriteToFile |                      ODataMessageWriter-Utf8-Async | 1,036.12 ms | 21.438 ms | 1.175 ms | 69000.0000 | 3000.0000 | 422,087 KB |
 WriteToFile |                   ODataMessageWriter-Utf8-Buffered |   320.17 ms |  2.583 ms | 0.142 ms | 41000.0000 | 1000.0000 | 255,010 KB |
 WriteToFile |               ODataMessageWriter-Utf8-NoValidation |   284.60 ms |  6.336 ms | 0.347 ms | 38000.0000 |         - | 233,508 KB |
 WriteToFile |         ODataMessageWriter-Utf8-NoValidation-Async |   965.36 ms | 58.489 ms | 3.206 ms | 65000.0000 | 2000.0000 | 400,967 KB |
 WriteToFile |                                     Utf8JsonWriter |    51.27 ms |  1.253 ms | 0.069 ms | 11000.0000 |         - |  68,512 KB |
 WriteToFile |                           Utf8JsonWriter-ArrayPool |    51.15 ms |  0.870 ms | 0.048 ms | 11000.0000 |         - |  68,278 KB |
 WriteToFile |              Utf8JsonWriter-ArrayPool-NoValidation |    48.95 ms |  1.317 ms | 0.072 ms | 11000.0000 |         - |  68,213 KB |
 WriteToFile |                              Utf8JsonWriter-Direct |    23.09 ms |  0.759 ms | 0.042 ms |          - |         - |     340 KB |
 WriteToFile |                    Utf8JsonWriter-Direct-ArrayPool |    22.66 ms |  1.699 ms | 0.093 ms |          - |         - |      47 KB |
 WriteToFile | Utf8JsonWriter-Direct-ArrayPool-ResourceGeneration |    25.91 ms |  0.645 ms | 0.035 ms |  1000.0000 |         - |   9,031 KB |
 WriteToFile |                 Utf8JsonWriter-Direct-NoValidation |    21.75 ms |  0.690 ms | 0.038 ms |          - |         - |     340 KB |
 WriteToFile |           Utf8JsonWriter-Direct-ResourceGeneration |    26.41 ms |  0.350 ms | 0.019 ms |  1000.0000 |         - |   9,324 KB |
 WriteToFile |                        Utf8JsonWriter-NoValidation |    49.21 ms |  0.322 ms | 0.018 ms | 11000.0000 |         - |  68,511 KB |

## ODataMessageWriter non-writer overhead

I first established what I called the non-writer overhead of the `ODataMessageWriter`. This is the cost of `ODataMessageWriter`'s processing and serialization activity apart from the actual writing to the destination stream. The rationale of measuring this overhead is to help estimate maximum impact we can have just by improving the `JsonWriter`. Theoretically, we can't do better than a writer that does nothing. For this reason I introduce a `NoOpWriter`, an implementation of `IJsonWriter` with empty methods. To approximate the non-writing overhead, I compare the baseline writer `ODataMessageWriter-Utf8` and the `NoOpWriter`-based `ODataMessageWriter-Utf8-NoOp`.

```
ODataMessageWriter-Utf8 -> Mean: 329.14ms, Allocated: 254,604 KB (~248MB)
ODataMessageWriter-NoOp -> Mean: 256.65ms, Allocated: 248,025KB (~242MB)
Diff -> Mean: ~72.49ms (~22%), Allocated: 6,579KB (~6.4MB) (~2.6%)
```

**TODO**: Comparing mean times is probably not the most reliable way of comparing the two scenarios. Find a better way to compare them. Or re-run the benchmarks a couple of times and check whether the diff is stable.

The `JsonWriter` accounts for about 22% of processing time and 2.6% of allocated memory. This is an approximate upperbound of much we can improve overall performance by just improving the `JsonWriter`. Of course we cannot get to this upperbound since the improved writer still has to write something. But this should still help us put our efforts in context.

22% improvement in processing time is quite attractive, assuming the diff estimate is reliable, but 2.6% of memory is much lower than expected. `ODataMessageWriter` is doing a lot of work in preprocessing, replacing the `JsonWriter` alone will not be sufficient to make a considerable dent to the overall overhead.

**TODO**: Confirm using profiler data

## ODataResource overhead

`ODataMessageWriter` does not serialize plain CLR objects (POCOs), it serializes `ODataItem`s (`ODataResource`, `ODataProperty`, etc.). This means the higher-level serializer (e.g. WebApi's `ODataSerializer`) has to convert its input objects (usually POCOs) to `ODataResource` objects.

This conversion results in allocations of `ODataItem`s as well as boxing of the primitive values (e.g. `int`s, `bool`s) into `object`s because `ODataProperty` values are of type `object`.

Basically, we are going from strongly-typed POCOs to dynamic `ODataItem`s. The `IJsonWriter.WriteValue` overloads are strongly-typed. This means that the boxed primitives inside `ODataProperty`s will have to be unboxed before calling the equivalent `IJsonWriter.WriteValue` overload.

`System.Text.Json` avoids such overhead by having strongly-typed converters. If we can implement such a model for OData serialization, we might able to shave off some overhead.

I tried to estimate this overhead by comparing OData `JsonWriter` with and without `ODataResource` conversion. I also added a third scenario that creates intermediate clones of the input objects. The idea was to simulate a hypoehtical scenario where we might need to create intermediate objects, but still keep things strongly-typed and without boxing/unboxing.

```
ODataJsonWriter-Utf8                      -> Mean: 65.11 ms Allocated: 74,841 KB (~74MB)
ODataJsonWriter-Direct-ResourceGeneration -> Mean: 39.76 ms Allocated: 15,660 KB (~15MB)
ODataJsonWriter-Direct                    -> Mean: 36.98 ms	Allocated: 6,676 KB (~6.5MB)

Diff-ResourceGeneration -> Mean: 25.35 ms (~39%) Allocated: 60181 KB (58.6 MB) (79%)
Diff -> Mean: 28.13 ms (43%) Allocated: 68,165 KB (~66.6MB) (91%)
```

We see a significant difference here. We can improve the `JsonWriter` writer by up to at least 43% processing time and 91% heap allocations if we were to eliminate the use of `ODataResource`.

But how does this impact the overall serialization? From the figures above we can estimate that `ODataResource` conversions take up about 28.13ms and 68,165KB. If we subtract those from the `ODataMessageWriter-Utf8` baseline, we get:

```
ODataMessageWriter-Utf8 -> Mean: 329.14ms, Allocated: 254,604 KB (~248MB)
ODataResource conversion estimate -> 28.13 ms (~8.5%), Allocated: 68,165KB (~66.MB) (~26.7%)

ODataMessageWriter without ODataResource conversion -> 301 ms Allocated: 186,439 KB (~182MB)
```

There's a non-neglible up to 8.5% improvement opportunity in processing time and a staggering 26.7% in heap memory allocations. This is significant.

## Total window of opportunity

Now let's combine this with the writer's cost to estimate the upperbound of the available window of opportunity for improvements:

```
ODataMessageWriter-Utf8 -> Mean: 329.14ms, Allocated: 254,604 KB (~248MB)
ODataMessageWriter-NoOp -> Mean: 256.65ms, Allocated: 248,025KB (~242MB)
ODataResource overhead estimate -> 28.13 ms (~8.5%), Allocated: 68,165KB (~66.MB) (~26.7%)

ODataMessageWriter without JsonWriter or ODataResource overhead -> 228.52 ms, Allocated: 179,860 KB (175.64MB)

Diff (with/without writer and resource overhead) -> Time: 100ms (30%) Allocated: 74,644 KB (~72.89 MB) (~29%)
```

This represents the upperbound of improvements we can achieve by optimizing the `JsonWriter` and eliminating `ODataResource` generation. This is not an estimate of how much we're likely to gain from said optimizations, but rather, if we want to achieve higher optimization impact, we should look at other places.

That said, 30% in both processing time and memory allocations represents huge opportunity that makes it worth the while to explore possible optimizations. The good thing about exploring optimizations in these areas is that they do not affect semantics of OData We can explore the optimizations without changing the expected behaviour, validations or overall semantic of an OData payload.

There are likely other bottlenecks within `ODataMessageWriter` that we should look to optimse, but they may require great consideration not to impact expected behaviour. Furthermore, if we make considerable improvements to the suggested areas, other opportunities for micro-optimizations will stand out because now they will account for a larger portion of the total overhead.

`ODataResource` elimination will likely be a much bigger effort than improving/replacing `JsonWriter`, it will introduce a new model for doing conversion and will require changes to higher-level serializers (like WebAPI's `ODataSerializer`).

## OData's `JsonWriter` vs `Utf8JsonWriter`

One of my proposed improvements for `JsonWriter` is to create an implementation of `IJsonWriter` using `Utf8JsonWriter`. The effort to doing that will be non-trivial (as outlined [in this document](./experiment-proposals.md#5-provide-a-custom-ijsonwriter-using-utf8jsonwriter-via-depdency-injection)). So first we compare `JsonWriter` and `Utf8JsonWriter` performance to help decide whether the effort would be worth it.

First, we do a head-to-head comparison of the two writers writing the same payload, without any validation or `ODataResource` generation. For `Utf8JsonWriter` I considered both the default `IBufferWriter<T>` (`ArrayBufferWriter<byte>`, which allocate array buffers on the heap) and a clone of the `PooledByteArrayBufferWriter` that's used internally by `JsonSerializer` (rents arrays from the share ArrayPool instead of allocating new ones).

```
ODataJsonWriter-Direct -> Time: 36.98 ms, Allocated: 6,676 KB (6.5MB)
Utf8JsonWriter-Direct-NoValidation -> 21.75 ms 340 KB
Utf8JsonWriter-Direct-ArrayPool -> Time: 22.66 ms, Allocated: 47 KB

Diff (without ArrayPool) -> Time: 15.23 ms (41%), Allocated: 6,336KB (6.2MB) (94.9%)
Diff (with ArrayPool) -> Time: 14.3 ms (38.7%), Allocated: 6,629 KB (6.47 MB) (99%)
```

We see that `Utf8JsonWriter` uses signficantly less memory (up to 99% when using the `ArrayPool`) and less time (41% less).

*TODO* also compare buffered versions of ODataJsonWriter-Direct.

The difference becomes less signficant when you account for `ODataResource` conversions:

```
ODataJsonWriter-Utf8 -> Time: 65.11 ms, Allocated: 74,841 KB (73MB)
ODataJsonWriter-Utf8-Buffered -> Time: 60.11 ms	1.821 ms, Allocated: 75,248 KB
Utf8JsonWriter-NoValidation -> Time: 49.21 ms, Allocated: 68,511 KB
Utf8JsonWriter-ArrayPool-NoValidation -> 48.95 ms, Allocated: 68,213 KB (66.6MB)

Diff (ODataJsonWriter-Utf8 vs Utf8JsonWriter-ArrayPool-NoValidation) ->
    Time: 16 ms (24.8%), Allocated: 6,628 KB (6.47MB) (8%)
```
The difference is still non-negligeable, but definitely not comparable to the opportunity available when `ODataResource` conversion is not a factor. Wrapping `JsonWriter`'s stream with a `BufferedStream` slightly improves time, but results in slightly more memory allocations.

Now let's see how much impact using `Utf8JsonWriter` could have on the overall writer. To estimate the impact, we add the cost of `Utf8JsonWriter` to the baseline `ODataMessageWriter-Utf8`.

```
ODataMessageWriter-Utf8 -> Mean: 329.14ms, Allocated: 254,604 KB (~248MB)
ODataMessageWriter-NoOp -> Mean: 256.65ms, Allocated: 248,025KB (~242MB)
Utf8JsonWriter-Direct-ArrayPool -> Time: 22.66 ms, Allocated: 47 KB

ODataMessageWriter with Utf8JsonWriter estimate -> Time: 279.31ms, Allocated: 248,072 KB

Diff -> 49.9 ms (15%), Allocated: 6,532 KB (6.38MB) (2.6%)
```

15% improvement less than our calculated upperbound (22%), but that is expected since the upperbound was based on doing something. 15% is probably a more realistic upperbound since it's possible that the actual implementation will have more going on.

2.6% improvement in memory is close to the upperbound we calculated. We can't expect to do better than this without optimizing other areas of `ODataMessageWriter`.

Now let's see what the potential impact is if we couple this with `ODataResource` elimination:

```
ODataMessageWriter-Utf8 -> Mean: 329.14ms, Allocated: 254,604 KB (~248MB)
ODataMessageWriter without JsonWriter or ODataResource overhead -> 228.52 ms, Allocated: 179,860 KB (175.64MB)
Utf8JsonWriter-Direct-ArrayPool -> Time: 22.66 ms, Allocated: 47 KB

ODataMessageWriter with Utf8JsonWriter but no ODataResource estimate -> Time: 251.18 ms, Allocated: 179,860 KB (175.64MB)

Diff -> Time: 77.96 ms (23.7 %), Allocated: 74,744 KB (72.3MB) (29%)
```

**TODO**: remove this code snippet
```python
def diff(x, y):
    meanX, allocX = x
    meanY, allocY = y
    meanDiff = meanX - meanY
    meanRatio = meanDiff / meanX
    allocDiff = allocX - allocY
    allocRatio = allocDiff/allocX
    allocXM, allocYM, allocDiffM = allocX/1024, allocY/1024, allocDiff/1024
    print("AllocX MB:", allocXM)              
    print("AllocY MB:", allocYM)
    print("Diff Mean:", meanDiff, "ms", "\t", "Ratio:", meanRatio)
    print("Diff Alloc:", allocDiff, "KB", allocDiffM, "MB", "\t", "Ratio:", allocRatio)
```