``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8665U CPU 1.90GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT


```
|              Method |    Categories | dataSize |         Mean |      Error |      StdDev |        Median | Ratio | RatioSD | Code Size |
|-------------------- |-------------- |--------- |-------------:|-----------:|------------:|--------------:|------:|--------:|----------:|
| **MemberWiseSumScalar** | **MemberWiseSum** |     **4096** |     **4.975 μs** |  **0.1233 μs** |   **0.3635 μs** |     **4.9436 μs** |  **1.00** |    **0.00** |     **184 B** |
|   MemberWiseSumSIMD | MemberWiseSum |     4096 |     4.052 μs |  0.3090 μs |   0.9111 μs |     3.9933 μs |  0.82 |    0.21 |     134 B |
|                     |               |          |              |            |             |               |       |         |           |
|       ArraySumSalar |      ArraySum |     4096 |     3.771 μs |  0.1445 μs |   0.4262 μs |     3.7618 μs |  1.00 |    0.00 |      51 B |
|        ArraySumSIMD |      ArraySum |     4096 |     1.035 μs |  0.1079 μs |   0.3078 μs |     0.9203 μs |  0.28 |    0.08 |     136 B |
|                     |               |          |              |            |             |               |       |         |           |
| **MemberWiseSumScalar** | **MemberWiseSum** |    **65536** |    **67.470 μs** |  **3.9889 μs** |  **11.0533 μs** |    **67.9839 μs** |  **1.00** |    **0.00** |     **184 B** |
|   MemberWiseSumSIMD | MemberWiseSum |    65536 |    36.167 μs |  1.1960 μs |   3.4123 μs |    35.0679 μs |  0.55 |    0.10 |     134 B |
|                     |               |          |              |            |             |               |       |         |           |
|       ArraySumSalar |      ArraySum |    65536 |    45.124 μs |  1.4357 μs |   4.1425 μs |    44.3719 μs |  1.00 |    0.00 |      51 B |
|        ArraySumSIMD |      ArraySum |    65536 |    14.721 μs |  1.5395 μs |   4.5391 μs |    13.1433 μs |  0.33 |    0.12 |     136 B |
|                     |               |          |              |            |             |               |       |         |           |
| **MemberWiseSumScalar** | **MemberWiseSum** |  **1048576** | **1,582.610 μs** | **36.5532 μs** | **106.0473 μs** | **1,561.2326 μs** |  **1.00** |    **0.00** |     **184 B** |
|   MemberWiseSumSIMD | MemberWiseSum |  1048576 |   991.435 μs | 19.1238 μs |  18.7821 μs |   982.2410 μs |  0.58 |    0.02 |     134 B |
|                     |               |          |              |            |             |               |       |         |           |
|       ArraySumSalar |      ArraySum |  1048576 |   683.266 μs | 13.0133 μs |  11.5360 μs |   684.4435 μs |  1.00 |    0.00 |      51 B |
|        ArraySumSIMD |      ArraySum |  1048576 |   281.997 μs | 23.0494 μs |  65.7611 μs |   289.8456 μs |  0.26 |    0.02 |     136 B |
