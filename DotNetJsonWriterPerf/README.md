# Comparing performance of different JSON writer APIs in .NET

Comparison between `Newtonsoft.Json.Linq`, `System.Text.Json` and `System.Text.Json.Nodes` APIs for
serialization.

| Method                         | dataCount | Mean           | Error        | StdDev       | Median         | Gen0       | Gen1       | Gen2      | Allocated    |
|------------------------------- |---------- |---------------:|-------------:|-------------:|---------------:|-----------:|-----------:|----------:|-------------:|
| SerializePayloadJToken         | 4         |     4,266.7 us |    126.25 us |    360.19 us |     4,160.4 us |   382.8125 |   328.1250 |         - |   2280.14 KB |
| SerializePayloadJsonNode       | 4         |     1,053.3 us |     34.38 us |     99.19 us |     1,031.0 us |   126.9531 |    99.6094 |   83.0078 |    471.77 KB |
| SerializeExtensibleJsonElement | 4         |     1,099.1 us |     25.53 us |     73.25 us |     1,093.0 us |   166.0156 |   166.0156 |  166.0156 |    629.51 KB |
| SerializeJsonElement           | 4         |     1,043.1 us |     20.84 us |     42.10 us |     1,028.3 us |   166.0156 |   166.0156 |  166.0156 |     628.1 KB |
| TraverseJToken                 | 4         |     3,557.6 us |     64.08 us |    112.23 us |     3,518.0 us |   402.3438 |   339.8438 |         - |   2330.08 KB |
| TraverseJsonNode               | 4         |     2,096.5 us |     33.52 us |     32.92 us |     2,096.9 us |   281.2500 |   187.5000 |   46.8750 |   1572.14 KB |
| TraverseJsonElement            | 4         |       878.0 us |     10.66 us |      8.90 us |       873.7 us |   124.0234 |   124.0234 |  124.0234 |    600.01 KB |
| SerializePayloadJToken         | 1000      | 2,481,009.2 us | 32,422.30 us | 27,074.09 us | 2,483,038.5 us | 96000.0000 | 50000.0000 | 4000.0000 |  568597.5 KB |
| SerializePayloadJsonNode       | 1000      |   254,267.0 us |  4,767.91 us |  5,490.73 us |   254,118.8 us |          - |          - |         - | 126816.04 KB |
| SerializeExtensibleJsonElement | 1000      |   237,092.4 us |  4,484.00 us |  9,159.62 us |   233,451.1 us |          - |          - |         - | 167821.04 KB |
| SerializeJsonElement           | 1000      |   237,008.7 us |  4,602.73 us |  6,452.39 us |   235,348.5 us |          - |          - |         - | 167819.63 KB |
| TraverseJToken                 | 1000      | 2,475,487.5 us | 49,276.64 us | 43,682.46 us | 2,476,945.6 us | 99000.0000 | 51000.0000 | 4000.0000 | 580474.79 KB |
| TraverseJsonNode               | 1000      | 1,269,606.2 us | 18,241.89 us | 15,232.80 us | 1,270,685.3 us | 57000.0000 | 30000.0000 | 3000.0000 |  391880.1 KB |
| TraverseJsonElement            | 1000      |   207,725.2 us |  4,118.42 us |  3,852.38 us |   207,330.4 us | 12500.0000 |          - |         - | 152000.87 KB |

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
