``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i7-8665U CPU 1.90GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]     : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  DefaultJob : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
  Job-GWQEDZ : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT

InvocationCount=1  

```
|             Method |        Job | UnrollFactor | dataSize |       Mean |      Error |     StdDev | Ratio | RatioSD |      Gen 0 |     Gen 1 |    Gen 2 |    Allocated |
|------------------- |----------- |------------- |--------- |-----------:|-----------:|-----------:|------:|--------:|-----------:|----------:|---------:|-------------:|
|          WriteJson | DefaultJob |           16 |     5000 |   9.900 ms |  0.1920 ms |  0.2692 ms |  1.00 |    0.00 |   859.3750 |  750.0000 | 750.0000 |   4068.88 KB |
|         WriteOData | DefaultJob |           16 |     5000 | 251.552 ms |  4.1174 ms |  3.8514 ms | 25.15 |    0.66 | 28000.0000 | 1000.0000 |        - | 121964.45 KB |
|     WriteODataSync | DefaultJob |           16 |     5000 | 207.125 ms |  3.9800 ms |  3.5282 ms | 20.70 |    0.67 | 24000.0000 |         - |        - | 104437.29 KB |
|                    |            |              |          |            |            |            |       |         |            |           |          |              |
|      WriteJsonFile | Job-GWQEDZ |            1 |     5000 |  15.596 ms |  0.7763 ms |  2.2023 ms |     ? |       ? |          - |         - |        - |    410.66 KB |
|     WriteODataFile | Job-GWQEDZ |            1 |     5000 | 313.366 ms | 12.2269 ms | 35.8595 ms |     ? |       ? | 28000.0000 | 1000.0000 |        - | 119509.02 KB |
| WriteODataSyncFile | Job-GWQEDZ |            1 |     5000 | 274.697 ms |  7.5745 ms | 22.0951 ms |     ? |       ? | 24000.0000 | 1000.0000 |        - | 100345.96 KB |
