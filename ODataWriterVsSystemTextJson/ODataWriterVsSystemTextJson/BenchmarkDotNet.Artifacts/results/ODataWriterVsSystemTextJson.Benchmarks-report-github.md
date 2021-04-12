``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-8665U CPU 1.90GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]     : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  DefaultJob : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  Job-ABHABG : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT

InvocationCount=1  

```
|             Method |        Job | UnrollFactor | dataSize |       Mean |       Error |      StdDev |     Median | Ratio | RatioSD |      Gen 0 |     Gen 1 |    Gen 2 |    Allocated |
|------------------- |----------- |------------- |--------- |-----------:|------------:|------------:|-----------:|------:|--------:|-----------:|----------:|---------:|-------------:|
|          **WriteJson** | **DefaultJob** |           **16** |     **1000** |   **1.988 ms** |   **0.0302 ms** |   **0.0268 ms** |   **1.995 ms** |  **1.00** |    **0.00** |   **109.3750** |  **109.3750** | **109.3750** |    **525.52 KB** |
|         WriteOData | DefaultJob |           16 |     1000 |  59.668 ms |   1.1631 ms |   1.6681 ms |  59.344 ms | 29.89 |    1.19 |  5000.0000 |         - |        - |  24490.41 KB |
|     WriteODataSync | DefaultJob |           16 |     1000 |  39.740 ms |   0.5698 ms |   0.4448 ms |  39.915 ms | 20.01 |    0.43 |  4923.0769 |  153.8462 |  76.9231 |  20595.88 KB |
|                    |            |              |          |            |             |             |            |       |         |            |           |          |              |
|      WriteJsonFile | Job-ABHABG |            1 |     1000 |   3.771 ms |   0.3180 ms |   0.8917 ms |   3.494 ms |     ? |       ? |          - |         - |        - |     98.05 KB |
|     WriteODataFile | Job-ABHABG |            1 |     1000 |  64.677 ms |   2.2464 ms |   6.5173 ms |  63.439 ms |     ? |       ? |  5000.0000 |         - |        - |  23937.82 KB |
| WriteODataSyncFile | Job-ABHABG |            1 |     1000 |  57.521 ms |   2.1010 ms |   6.0281 ms |  56.991 ms |     ? |       ? |  4000.0000 | 1000.0000 |        - |  20088.62 KB |
|                    |            |              |          |            |             |             |            |       |         |            |           |          |              |
|          **WriteJson** | **DefaultJob** |           **16** |     **5000** |   **9.479 ms** |   **0.1632 ms** |   **0.1526 ms** |   **9.418 ms** |  **1.00** |    **0.00** |   **828.1250** |  **718.7500** | **718.7500** |    **4068.3 KB** |
|         WriteOData | DefaultJob |           16 |     5000 | 236.482 ms |   3.0738 ms |   2.8753 ms | 236.680 ms | 24.95 |    0.48 | 28000.0000 | 1000.0000 |        - | 121955.65 KB |
|     WriteODataSync | DefaultJob |           16 |     5000 | 210.549 ms |   4.0937 ms |   4.0206 ms | 210.272 ms | 22.22 |    0.62 | 24000.0000 |         - |        - | 104437.29 KB |
|                    |            |              |          |            |             |             |            |       |         |            |           |          |              |
|      WriteJsonFile | Job-ABHABG |            1 |     5000 |  15.386 ms |   0.9091 ms |   2.6520 ms |  14.725 ms |     ? |       ? |          - |         - |        - |    410.66 KB |
|     WriteODataFile | Job-ABHABG |            1 |     5000 | 341.139 ms |  24.7906 ms |  69.9224 ms | 322.275 ms |     ? |       ? | 28000.0000 | 2000.0000 |        - | 119509.02 KB |
| WriteODataSyncFile | Job-ABHABG |            1 |     5000 | 275.562 ms |   8.6091 ms |  25.1130 ms | 275.487 ms |     ? |       ? | 24000.0000 | 1000.0000 |        - | 100345.96 KB |
|                    |            |              |          |            |             |             |            |       |         |            |           |          |              |
|          **WriteJson** | **DefaultJob** |           **16** |    **10000** |  **18.647 ms** |   **0.2563 ms** |   **0.2398 ms** |  **18.706 ms** |  **1.00** |    **0.00** |   **937.5000** |  **750.0000** | **750.0000** |      **8149 KB** |
|         WriteOData | DefaultJob |           16 |    10000 | 908.103 ms | 112.1642 ms | 330.7187 ms | 867.588 ms | 26.70 |    2.52 | 55000.0000 | 2000.0000 |        - | 243963.89 KB |
|     WriteODataSync | DefaultJob |           16 |    10000 | 699.142 ms |  31.6311 ms |  87.1212 ms | 690.979 ms | 37.54 |    2.97 | 49000.0000 |         - |        - | 208854.99 KB |
|                    |            |              |          |            |             |             |            |       |         |            |           |          |              |
|      WriteJsonFile | Job-ABHABG |            1 |    10000 |  34.629 ms |   1.6557 ms |   4.8558 ms |  32.933 ms |     ? |       ? |          - |         - |        - |    816.33 KB |
|     WriteODataFile | Job-ABHABG |            1 |    10000 | 704.676 ms |  39.3033 ms | 112.1344 ms | 673.265 ms |     ? |       ? | 55000.0000 | 2000.0000 |        - | 238992.23 KB |
| WriteODataSyncFile | Job-ABHABG |            1 |    10000 | 621.102 ms |  36.1759 ms | 105.5269 ms | 601.100 ms |     ? |       ? | 49000.0000 | 1000.0000 |        - | 200667.64 KB |
