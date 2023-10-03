# Comparing performance of different JSON writer APIs in .NET

Comparison between `Newtonsoft.Json.Linq`, `System.Text.Json` and `System.Text.Json.Nodes` APIs for
serialization.

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100-rc.1.23463.5
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

| Method                         | personCount | Mean      | Error     | StdDev    | Median    | Gen0   | Gen1   | Allocated |
|------------------------------- |------------ |----------:|----------:|----------:|----------:|-------:|-------:|----------:|
| SerializePayloadJsonNet        | 4           |  3.948 μs | 0.0839 μs | 0.2408 μs |  3.893 μs | 1.9226 |      - |   8.11 KB |
| SerializePayloadJsonNode       | 4           |  2.303 μs | 0.0668 μs | 0.1939 μs |  2.243 μs | 0.4616 |      - |   1.95 KB |
| SerializeExtensibleJsonElement | 4           |  1.567 μs | 0.0306 μs | 0.0567 μs |  1.559 μs | 0.4749 |      - |   2.01 KB |
| SerializeJsonElement           | 4           |  5.458 μs | 0.1085 μs | 0.2515 μs |  5.371 μs | 1.5564 |      - |   6.58 KB |
| TraverseJToken                 | 4           | 15.747 μs | 0.5986 μs | 1.6385 μs | 15.382 μs | 3.2349 | 0.0305 |  13.73 KB |
| TraverseJsonNode               | 4           |  9.885 μs | 0.1964 μs | 0.5277 μs |  9.757 μs | 1.8616 |      - |   7.88 KB |
| SerializePayloadJsonNet        | 1000        |  3.943 μs | 0.1046 μs | 0.3002 μs |  3.904 μs | 1.9226 |      - |   8.11 KB |
| SerializePayloadJsonNode       | 1000        |  2.258 μs | 0.0674 μs | 0.1944 μs |  2.220 μs | 0.4616 |      - |   1.95 KB |
| SerializeExtensibleJsonElement | 1000        |  1.601 μs | 0.0321 μs | 0.0717 μs |  1.598 μs | 0.4749 |      - |   2.01 KB |
| SerializeJsonElement           | 1000        |  5.521 μs | 0.1099 μs | 0.1744 μs |  5.535 μs | 1.5564 |      - |   6.58 KB |
| TraverseJToken                 | 1000        | 15.409 μs | 0.4255 μs | 1.2345 μs | 14.992 μs | 3.2349 | 0.0305 |  13.73 KB |
| TraverseJsonNode               | 1000        |  9.874 μs | 0.1935 μs | 0.2955 μs |  9.785 μs | 1.8616 |      - |   7.88 KB |

// * Warnings *
MultimodalDistribution
  Benchmarks.SerializePayloadJsonNet: Default  -> It seems that the distribution can have several modes (mValue = 3.07)
  Benchmarks.TraverseJToken: Default           -> It seems that the distribution is bimodal (mValue = 3.38)
  Benchmarks.SerializePayloadJsonNode: Default -> It seems that the distribution can have several modes (mValue = 2.88)

// * Hints *
Outliers
  Benchmarks.SerializePayloadJsonNet: Default        -> 5 outliers were removed (4.76 μs..5.24 μs)
  Benchmarks.SerializePayloadJsonNode: Default       -> 3 outliers were removed (2.87 μs..3.02 μs)
  Benchmarks.SerializeExtensibleJsonElement: Default -> 1 outlier  was  removed (1.80 μs)
  Benchmarks.SerializeJsonElement: Default           -> 1 outlier  was  removed (6.62 μs)
  Benchmarks.TraverseJToken: Default                 -> 13 outliers were removed (21.08 μs..24.46 μs)
  Benchmarks.TraverseJsonNode: Default               -> 4 outliers were removed (12.19 μs..14.51 μs)
  Benchmarks.SerializePayloadJsonNet: Default        -> 5 outliers were removed (4.93 μs..5.24 μs)
  Benchmarks.SerializePayloadJsonNode: Default       -> 4 outliers were removed (2.92 μs..3.40 μs)
  Benchmarks.SerializeExtensibleJsonElement: Default -> 4 outliers were removed (1.82 μs..1.91 μs)
  Benchmarks.SerializeJsonElement: Default           -> 1 outlier  was  removed (6.22 μs)
  Benchmarks.TraverseJToken: Default                 -> 3 outliers were removed (19.64 μs..20.51 μs)
  Benchmarks.TraverseJsonNode: Default               -> 3 outliers were removed (10.70 μs..10.90 μs)

// * Legends *
  personCount : Value of the 'personCount' parameter
  Mean        : Arithmetic mean of all measurements
  Error       : Half of 99.9% confidence interval
  StdDev      : Standard deviation of all measurements
  Median      : Value separating the higher half of all measurements (50th percentile)
  Gen0        : GC Generation 0 collects per 1000 operations
  Gen1        : GC Generation 1 collects per 1000 operations
  Allocated   : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 μs        : 1 Microsecond (0.000001 sec)