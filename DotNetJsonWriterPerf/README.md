# Comparing performance of different JSON writer APIs in .NET

Comparison between `Newtonsoft.Json.Linq`, `System.Text.Json` and `System.Text.Json.Nodes` APIs for
serialization.

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100-rc.1.23463.5
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

| Method                         | dataCount | Mean           | Error        | StdDev       | Gen0        | Gen1       | Gen2      | Allocated    |
|------------------------------- |---------- |---------------:|-------------:|-------------:|------------:|-----------:|----------:|-------------:|
| SerializePayloadJToken         | 4         |     4,654.6 us |    154.00 us |    454.08 us |    375.0000 |   343.7500 |         - |    2289.4 KB |
| SerializePayloadJsonNode       | 4         |       982.1 us |     18.46 us |     28.19 us |    126.9531 |    99.6094 |   82.0313 |    475.54 KB |
| SerializeExtensibleJsonElement | 4         |       995.7 us |     15.43 us |     12.88 us |    166.0156 |   166.0156 |  166.0156 |    629.51 KB |
| SerializeJsonElement           | 4         |     1,043.0 us |     20.71 us |     32.84 us |    166.0156 |   166.0156 |  166.0156 |     628.1 KB |
| TraverseJToken                 | 4         |     3,755.0 us |     72.85 us |    111.25 us |    414.0625 |   328.1250 |         - |   2363.36 KB |
| TraverseJsonNode               | 4         |     2,187.6 us |     43.47 us |     66.38 us |    285.1563 |   167.9688 |   46.8750 |    1594.8 KB |
| TraverseJsonElement            | 4         |       913.1 us |     11.26 us |      9.98 us |    124.0234 |   124.0234 |  124.0234 |    603.06 KB |
| SerializePayloadJToken         | 1000      | 2,512,583.7 us | 49,644.46 us | 46,437.46 us |  97000.0000 | 50000.0000 | 4000.0000 |  571696.2 KB |
| SerializePayloadJsonNode       | 1000      |   261,621.4 us |  4,666.43 us |  6,984.50 us |           - |          - |         - | 128025.77 KB |
| SerializeExtensibleJsonElement | 1000      |   235,467.0 us |  4,643.64 us |  6,509.73 us |           - |          - |         - | 167821.04 KB |
| SerializeJsonElement           | 1000      |   244,851.4 us |  2,873.78 us |  3,836.42 us |           - |          - |         - | 167819.63 KB |
| TraverseJToken                 | 1000      | 2,482,471.4 us | 49,006.54 us | 58,338.80 us | 101000.0000 | 52000.0000 | 4000.0000 | 591573.54 KB |
| TraverseJsonNode               | 1000      | 1,335,478.0 us | 26,611.71 us | 47,302.29 us |  58000.0000 | 30000.0000 | 3000.0000 | 399428.35 KB |
| TraverseJsonElement            | 1000      |   212,667.1 us |  3,372.30 us |  4,943.07 us |  12500.0000 |          - |         - | 153015.48 KB |

**Legends**

  dataCount : Value of the 'dataCount' parameter

  Mean      : Arithmetic mean of all measurements

  Error     : Half of 99.9% confidence interval

  StdDev    : Standard deviation of all measurements

  Median    : Value separating the higher half of all measurements (50th percentile)

  Gen0      : GC Generation 0 collects per 1000 operations

  Gen1      : GC Generation 1 collects per 1000 operations

  Gen2      : GC Generation 2 collects per 1000 operations

  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)

  1 us      : 1 Microsecond (0.000001 sec)
