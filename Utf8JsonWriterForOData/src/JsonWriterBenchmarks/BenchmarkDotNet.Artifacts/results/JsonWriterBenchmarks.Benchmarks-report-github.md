``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8665U CPU 1.90GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.201
  [Host]   : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT
  ShortRun : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT

Job=ShortRun  InvocationCount=1  IterationCount=3  
LaunchCount=1  UnrollFactor=1  WarmupCount=3  

```
|      Method |                                         WriterName |        Mean |        Error |     StdDev |      Gen 0 |     Gen 1 |  Allocated |
|------------ |--------------------------------------------------- |------------:|-------------:|-----------:|-----------:|----------:|-----------:|
| **WriteToFile** |                                     **JsonSerializer** |   **687.74 ms** |    **551.91 ms** |  **30.252 ms** |          **-** |         **-** |      **62 KB** |
| **WriteToFile** |                                         **NoOpWriter** |   **148.72 ms** |    **245.17 ms** |  **13.439 ms** | **16000.0000** |         **-** |  **68,168 KB** |
| **WriteToFile** |                                   **NoOpWriter-Async** |   **147.36 ms** |     **89.11 ms** |   **4.884 ms** | **16000.0000** |         **-** |  **68,168 KB** |
| **WriteToFile** |                              **ODataJsonWriter-Utf16** |   **283.90 ms** |    **127.54 ms** |   **6.991 ms** | **18000.0000** |         **-** |  **74,836 KB** |
| **WriteToFile** |                        **ODataJsonWriter-Utf16-Async** | **1,851.30 ms** |  **1,483.99 ms** |  **81.342 ms** | **36000.0000** |         **-** | **147,532 KB** |
| **WriteToFile** |                               **ODataJsonWriter-Utf8** |   **260.76 ms** |    **161.16 ms** |   **8.834 ms** | **18000.0000** |         **-** |  **74,841 KB** |
| **WriteToFile** |                         **ODataJsonWriter-Utf8-Async** | **2,662.04 ms** |  **5,018.13 ms** | **275.061 ms** | **36000.0000** |         **-** | **147,538 KB** |
| **WriteToFile** |                      **ODataJsonWriter-Utf8-Buffered** |   **213.82 ms** |    **155.03 ms** |   **8.497 ms** | **18000.0000** | **1000.0000** |  **75,247 KB** |
| **WriteToFile** |                **ODataJsonWriter-Utf8-Buffered-Async** | **1,596.16 ms** |  **1,805.49 ms** |  **98.965 ms** | **35000.0000** | **1000.0000** | **142,284 KB** |
| **WriteToFile** |                            **ODataMessageWriter-NoOp** |   **605.07 ms** |    **269.05 ms** |  **14.748 ms** | **60000.0000** |         **-** | **248,044 KB** |
| **WriteToFile** |                      **ODataMessageWriter-NoOp-Async** |   **867.31 ms** |    **152.46 ms** |   **8.357 ms** | **66000.0000** | **1000.0000** | **272,279 KB** |
| **WriteToFile** |               **ODataMessageWriter-NoOp-NoValidation** |   **525.55 ms** |    **314.99 ms** |  **17.266 ms** | **55000.0000** |         **-** | **226,947 KB** |
| **WriteToFile** |         **ODataMessageWriter-NoOp-NoValidation-Async** |   **733.15 ms** |     **72.00 ms** |   **3.946 ms** | **61000.0000** |         **-** | **251,183 KB** |
| **WriteToFile** |                           **ODataMessageWriter-Utf16** |   **654.28 ms** |    **216.75 ms** |  **11.881 ms** | **62000.0000** |         **-** | **254,606 KB** |
| **WriteToFile** |                     **ODataMessageWriter-Utf16-Async** | **2,941.95 ms** |    **683.37 ms** |  **37.458 ms** | **87000.0000** | **6000.0000** | **429,607 KB** |
| **WriteToFile** |                            **ODataMessageWriter-Utf8** |   **573.54 ms** |    **257.60 ms** |  **14.120 ms** | **62000.0000** |         **-** | **254,611 KB** |
| **WriteToFile** |                      **ODataMessageWriter-Utf8-Async** | **3,017.55 ms** |    **862.72 ms** |  **47.289 ms** | **94000.0000** | **2000.0000** | **422,076 KB** |
| **WriteToFile** |                   **ODataMessageWriter-Utf8-Buffered** |   **631.50 ms** |    **129.81 ms** |   **7.115 ms** | **62000.0000** |         **-** | **255,018 KB** |
| **WriteToFile** |               **ODataMessageWriter-Utf8-NoValidation** |   **716.30 ms** |  **1,116.74 ms** |  **61.212 ms** | **57000.0000** |         **-** | **233,515 KB** |
| **WriteToFile** |         **ODataMessageWriter-Utf8-NoValidation-Async** | **4,575.34 ms** | **15,089.53 ms** | **827.108 ms** | **89000.0000** | **2000.0000** | **400,969 KB** |
| **WriteToFile** |                                     **Utf8JsonWriter** |   **297.99 ms** |  **1,155.81 ms** |  **63.354 ms** | **16000.0000** |         **-** |  **68,511 KB** |
| **WriteToFile** |                           **Utf8JsonWriter-ArrayPool** |   **244.43 ms** |    **322.23 ms** |  **17.662 ms** | **16000.0000** | **1000.0000** |  **68,532 KB** |
| **WriteToFile** |              **Utf8JsonWriter-ArrayPool-NoValidation** |   **500.99 ms** |  **2,303.54 ms** | **126.265 ms** | **16000.0000** | **1000.0000** |  **68,548 KB** |
| **WriteToFile** |                              **Utf8JsonWriter-Direct** |   **237.54 ms** |    **562.06 ms** |  **30.808 ms** |          **-** |         **-** |     **339 KB** |
| **WriteToFile** |                    **Utf8JsonWriter-Direct-ArrayPool** |   **118.96 ms** |    **229.83 ms** |  **12.598 ms** |          **-** |         **-** |     **366 KB** |
| **WriteToFile** | **Utf8JsonWriter-Direct-ArrayPool-ResourceGeneration** |    **91.22 ms** |    **127.84 ms** |   **7.008 ms** |  **2000.0000** |         **-** |   **9,351 KB** |
| **WriteToFile** |                 **Utf8JsonWriter-Direct-NoValidation** |   **106.53 ms** |    **253.46 ms** |  **13.893 ms** |          **-** |         **-** |     **339 KB** |
| **WriteToFile** |           **Utf8JsonWriter-Direct-ResourceGeneration** |    **96.38 ms** |     **16.93 ms** |   **0.928 ms** |  **2000.0000** |         **-** |   **9,324 KB** |
| **WriteToFile** |                        **Utf8JsonWriter-NoValidation** |   **325.92 ms** |  **1,298.01 ms** |  **71.148 ms** | **16000.0000** |         **-** |  **68,511 KB** |
