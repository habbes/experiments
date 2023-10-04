# Comparing performance of different JSON writer APIs in .NET

Comparison between `Newtonsoft.Json.Linq`, `System.Text.Json` and `System.Text.Json.Nodes` APIs for
serialization.

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3324/22H2/2022Update)
Intel Xeon W-2123 CPU 3.60GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100-rc.1.23463.5
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

| Method                         | dataCount | Mean           | Error        | StdDev       | Median         | Gen0        | Gen1       | Gen2      | Allocated    |
|------------------------------- |---------- |---------------:|-------------:|-------------:|---------------:|------------:|-----------:|----------:|-------------:|
| SerializePayloadJToken         | 4         |     3,929.1 us |     52.92 us |     44.19 us |     3,920.0 us |    378.9063 |   355.4688 |         - |   2289.39 KB |
| SerializePayloadJsonNode       | 4         |       943.8 us |     16.78 us |     21.81 us |       933.0 us |    127.9297 |   100.5859 |   83.0078 |    475.52 KB |
| SerializeExtensibleJsonElement | 4         |       964.9 us |      7.94 us |      6.20 us |       964.7 us |    166.0156 |   166.0156 |  166.0156 |    629.51 KB |
| SerializeJsonElement           | 4         |     1,001.0 us |     11.78 us |     10.44 us |     1,001.6 us |    166.0156 |   166.0156 |  166.0156 |     628.1 KB |
| TraverseJToken                 | 4         |     4,333.4 us |    142.45 us |    420.02 us |     4,586.6 us |    414.0625 |   312.5000 |         - |   2363.37 KB |
| TraverseJsonNode               | 4         |     2,121.7 us |     36.84 us |     55.14 us |     2,114.9 us |    304.6875 |   207.0313 |   42.9688 |   1594.85 KB |
| TraverseJsonElement            | 4         |       938.0 us |     18.37 us |     34.95 us |       932.5 us |    124.0234 |   124.0234 |  124.0234 |    603.06 KB |
| SerializePayloadJToken         | 1000      | 2,504,948.9 us | 39,978.89 us | 35,440.24 us | 2,499,998.0 us |  97000.0000 | 50000.0000 | 4000.0000 | 571696.23 KB |
| SerializePayloadJsonNode       | 1000      |   256,216.1 us |  4,783.79 us |  7,724.94 us |   253,385.3 us |           - |          - |         - | 128025.77 KB |
| SerializeExtensibleJsonElement | 1000      |   234,469.2 us |  2,141.78 us |  2,291.68 us |   234,337.8 us |           - |          - |         - | 167821.04 KB |
| SerializeJsonElement           | 1000      |   251,918.6 us |  4,924.28 us | 10,808.90 us |   247,114.8 us |           - |          - |         - | 167819.63 KB |
| TraverseJToken                 | 1000      | 2,457,853.5 us | 49,045.72 us | 58,385.44 us | 2,450,692.0 us | 101000.0000 | 52000.0000 | 4000.0000 | 591573.79 KB |
| TraverseJsonNode               | 1000      | 1,333,522.3 us | 26,377.30 us | 54,473.71 us | 1,320,342.2 us |  58000.0000 | 30000.0000 | 3000.0000 |  399428.2 KB |
| TraverseJsonElement            | 1000      |   209,719.7 us |  2,014.57 us |  2,474.07 us |   209,056.4 us |  12500.0000 |          - |         - | 153015.48 KB |

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
