``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8665U CPU 1.90GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT
  Job-HNBPCL : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT

InvocationCount=1  UnrollFactor=1  

```
|                                              Method |       Mean |      Error |      StdDev |     Median | Ratio | RatioSD |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|---------------------------------------------------- |-----------:|-----------:|------------:|-----------:|------:|--------:|-----------:|----------:|----------:|----------:|
|                                JsonSerializer_Write |   8.854 ms |  0.1670 ms |   0.3631 ms |   8.847 ms |  0.04 |    0.01 |          - |         - |         - |      4 MB |
|                          Utf8JsonWriter_WriteMemory |  29.630 ms |  2.4805 ms |   7.3139 ms |  33.088 ms |  0.15 |    0.05 |  4000.0000 | 2000.0000 | 1000.0000 |     19 MB |
|              Utf8JsonWriterNoValidation_WriteMemory |  31.939 ms |  1.5636 ms |   4.5612 ms |  33.009 ms |  0.16 |    0.05 |  4000.0000 | 2000.0000 | 1000.0000 |     19 MB |
|                     ODataJsonWriterUtf8_WriteMemory |  23.438 ms |  0.4673 ms |   1.3333 ms |  23.201 ms |  0.12 |    0.03 |  3000.0000 | 1000.0000 |         - |     19 MB |
|                    ODataJsonWriterUtf16_WriteMemory |  33.122 ms |  1.8802 ms |   5.5143 ms |  31.171 ms |  0.17 |    0.06 |  4000.0000 | 1000.0000 | 1000.0000 |     23 MB |
|             ODataJsonWriterUtf8Buffered_WriteMemory |  26.646 ms |  0.7779 ms |   2.2815 ms |  26.349 ms |  0.13 |    0.04 |  3000.0000 | 1000.0000 |         - |     25 MB |
|                ODataJsonWriterUtf8Async_WriteMemory | 176.305 ms | 16.7132 ms |  49.2793 ms | 179.337 ms |  0.91 |    0.45 |  7000.0000 | 1000.0000 |         - |     32 MB |
|               ODataJsonWriterUtf16Async_WriteMemory | 241.036 ms | 13.4582 ms |  39.6817 ms | 235.281 ms |  1.22 |    0.41 |  8000.0000 | 2000.0000 | 1000.0000 |     36 MB |
|         ODataJsonWriterUtf8BufferdAsync_WriteMemory | 224.636 ms | 21.7107 ms |  64.0145 ms | 242.533 ms |  1.14 |    0.50 |  7000.0000 |         - |         - |     33 MB |
|                  ODataMessageWriterUtf8_WriteMemory | 213.120 ms | 17.8739 ms |  52.7015 ms | 220.673 ms |  1.00 |    0.00 | 13000.0000 | 1000.0000 |         - |     59 MB |
|                 ODataMessageWriterUtf16_WriteMemory | 226.174 ms | 20.0885 ms |  59.2314 ms | 226.208 ms |  1.12 |    0.37 | 14000.0000 | 2000.0000 | 1000.0000 |     63 MB |
|      ODataMessageWriterUtf8NoValidation_WriteMemory | 185.398 ms | 16.6749 ms |  49.1664 ms | 169.115 ms |  0.94 |    0.37 | 12000.0000 |         - |         - |     54 MB |
|          ODataMessageWriterUtf8Buffered_WriteMemory | 180.710 ms | 12.9002 ms |  37.8341 ms | 181.309 ms |  0.91 |    0.31 | 13000.0000 | 1000.0000 |         - |     59 MB |
|                  ODataMessageWriterNoOp_WriteMemory | 122.040 ms |  7.8903 ms |  22.3836 ms | 114.830 ms |  0.61 |    0.19 | 13000.0000 |         - |         - |     53 MB |
|      ODataMessageWriterNoOpNoValodation_WriteMemory |  88.877 ms |  1.7604 ms |   2.6883 ms |  88.288 ms |  0.43 |    0.09 | 12000.0000 |         - |         - |     49 MB |
|             ODataMessageWriterUtf8Async_WriteMemory | 754.469 ms | 57.6632 ms | 170.0212 ms | 691.820 ms |  3.68 |    0.91 | 21000.0000 |         - |         - |     89 MB |
|            ODataMessageWriterUtf16Async_WriteMemory | 609.082 ms | 11.0666 ms |  10.3517 ms | 610.080 ms |  2.72 |    0.58 | 22000.0000 | 1000.0000 | 1000.0000 |     94 MB |
| ODataMessageWriterUtf8NoValidationAsync_WriteMemory | 617.979 ms | 23.9109 ms |  66.6541 ms | 586.225 ms |  3.13 |    1.09 | 20000.0000 |         - |         - |     85 MB |
|             ODataMessageWriterNoOpAsync_WriteMemory | 840.330 ms | 51.4270 ms | 151.6338 ms | 797.751 ms |  4.25 |    1.47 | 21000.0000 | 1000.0000 |         - |     89 MB |
| ODataMessageWriterNoOpNoValidationAsync_WriteMemory | 577.150 ms |  4.5376 ms |   3.5427 ms | 577.042 ms |  2.64 |    0.55 | 20000.0000 |         - |         - |     85 MB |
