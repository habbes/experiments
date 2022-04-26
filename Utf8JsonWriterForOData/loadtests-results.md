# Loadtests results

## Results summary

Results on lab machines:

|Writer | Total Requests | Avg latency (ms) | Avg. RPS
--------|----------------|------------------|---------
ODataMessageWriter-Utf8 | 239,820 | 16 | 7,993
ODataMessageWriter-NoOp | 288,813 | 13 | 9,626
ODataMessageWriter-Utf8-Async | 100,031 | 38 | 3,332
ODataJsonWriter-Utf8 | 786,545 | 4.9 | 26,227
Utf8JsonWriter-ArrayPool-NoValidation | 959,890 | 3.4 | 32,007
ODataJsonWriter-Utf8-Async | 269,446 | 14 | 8,983
ODataJsonWriter-Direct | 1,189,732 | 3 | 39,689
Utf8JsonWriter-Direct-ArrayPool-NoValidation | 1,678,664 | 2.3 | 55,984
ODataJsonWriter-Direct-Async | 378,460 | 10 | 27,055

## Raw results
### ODataMessageWriter-Utf8
```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataMessageWriter-Utf8 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,206         |
| Working Set (MB)                        | 197           |
| Private Memory (MB)                     | 342           |
| Build Time (ms)                         | 1,701         |
| Start Time (ms)                         | 265           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 102           |
| Max Working Set (MB)                    | 205           |
| Max GC Heap Size (MB)                   | 105           |
| Size of committed memory by the GC (MB) | 149           |
| Max Number of Gen 0 GCs / sec           | 59.00         |
| Max Number of Gen 1 GCs / sec           | 12.00         |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 7.00          |
| Max Gen 0 Size (B)                      | 8,324,800     |
| Max Gen 1 Size (B)                      | 6,106,328     |
| Max Gen 2 Size (B)                      | 12,975,552    |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,256,224     |
| Max Allocation Rate (B/sec)             | 5,206,099,192 |
| Max GC Heap Fragmentation               | 56            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 5             |
| Max ThreadPool Threads Count            | 34            |
| Max ThreadPool Queue Length             | 61            |
| Max ThreadPool Items (#/s)              | 20,504        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 369,987       |
| Methods Jitted                          | 5,066         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 13      |
| Cores usage (%)        | 158     |
| Working Set (MB)       | 41      |
| Private Memory (MB)    | 118     |
| Start Time (ms)        | 90      |
| First Request (ms)     | 169     |
| Requests               | 239,820 |
| Bad responses          | 0       |
| Latency 50th (us)      | 15,976  |
| Latency 75th (us)      | 16,236  |
| Latency 90th (us)      | 16,653  |
| Latency 95th (us)      | 16,989  |
| Latency 99th (us)      | 17,916  |
| Mean latency (us)      | 16,008  |
| Max latency (us)       | 144,814 |
| Requests/sec           | 7,993   |
| Requests/sec (max)     | 10,528  |
| Read throughput (MB/s) | 110.18  |
```

### ODataMessageWriter-Utf8-NoOp

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataMessageWriter-NoOp --application.options.counterProviders System.Runtime
```
```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,202         |
| Working Set (MB)                        | 184           |
| Private Memory (MB)                     | 341           |
| Build Time (ms)                         | 2,536         |
| Start Time (ms)                         | 267           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 100           |
| Max Working Set (MB)                    | 192           |
| Max GC Heap Size (MB)                   | 98            |
| Size of committed memory by the GC (MB) | 121           |
| Max Number of Gen 0 GCs / sec           | 70.00         |
| Max Number of Gen 1 GCs / sec           | 19.00         |
| Max Number of Gen 2 GCs / sec           | 2.00          |
| Max Time in GC (%)                      | 11.00         |
| Max Gen 0 Size (B)                      | 7,528,368     |
| Max Gen 1 Size (B)                      | 11,802,000    |
| Max Gen 2 Size (B)                      | 15,187,144    |
| Max LOH Size (B)                        | 6,484,112     |
| Max POH Size (B)                        | 600,032       |
| Max Allocation Rate (B/sec)             | 6,181,503,984 |
| Max GC Heap Fragmentation               | 49            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 40            |
| Max ThreadPool Threads Count            | 37            |
| Max ThreadPool Queue Length             | 81            |
| Max ThreadPool Items (#/s)              | 22,841        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 372,769       |
| Methods Jitted                          | 5,139         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 9       |
| Cores usage (%)        | 109     |
| Working Set (MB)       | 42      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 88      |
| First Request (ms)     | 169     |
| Requests               | 288,813 |
| Bad responses          | 0       |
| Latency 50th (us)      | 13,131  |
| Latency 75th (us)      | 14,119  |
| Latency 90th (us)      | 15,511  |
| Latency 95th (us)      | 16,689  |
| Latency 99th (us)      | 19,856  |
| Mean latency (us)      | 13,291  |
| Max latency (us)       | 150,533 |
| Requests/sec           | 9,626   |
| Requests/sec (max)     | 12,687  |
| Read throughput (MB/s) | 1.28    |
```

### ODataMessageWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataMessageWriter-Utf8-Async --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,204         |
| Working Set (MB)                        | 188           |
| Private Memory (MB)                     | 343           |
| Build Time (ms)                         | 2,512         |
| Start Time (ms)                         | 269           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 103           |
| Max Working Set (MB)                    | 203           |
| Max GC Heap Size (MB)                   | 93            |
| Size of committed memory by the GC (MB) | 143           |
| Max Number of Gen 0 GCs / sec           | 42.00         |
| Max Number of Gen 1 GCs / sec           | 15.00         |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 4.00          |
| Max Gen 0 Size (B)                      | 7,989,496     |
| Max Gen 1 Size (B)                      | 13,640,224    |
| Max Gen 2 Size (B)                      | 12,293,696    |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,898,888     |
| Max Allocation Rate (B/sec)             | 3,774,487,728 |
| Max GC Heap Fragmentation               | 53            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 10            |
| Max ThreadPool Threads Count            | 33            |
| Max ThreadPool Queue Length             | 80            |
| Max ThreadPool Items (#/s)              | 614,642       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 474,922       |
| Methods Jitted                          | 6,077         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 5       |
| Cores usage (%)        | 57      |
| Working Set (MB)       | 41      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 85      |
| First Request (ms)     | 208     |
| Requests               | 100,031 |
| Bad responses          | 0       |
| Latency 50th (us)      | 37,780  |
| Latency 75th (us)      | 40,835  |
| Latency 90th (us)      | 46,066  |
| Latency 95th (us)      | 49,849  |
| Latency 99th (us)      | 56,977  |
| Mean latency (us)      | 38,404  |
| Max latency (us)       | 171,514 |
| Requests/sec           | 3,332   |
| Requests/sec (max)     | 6,398   |
| Read throughput (MB/s) | 45.64   |
```

### ODataJsonWriter-Utf8

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Utf8 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 99            |
| Cores usage (%)                         | 1,189         |
| Working Set (MB)                        | 209           |
| Private Memory (MB)                     | 224           |
| Build Time (ms)                         | 7,231         |
| Start Time (ms)                         | 269           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 100           |
| Max Working Set (MB)                    | 261           |
| Max GC Heap Size (MB)                   | 104           |
| Size of committed memory by the GC (MB) | 219           |
| Max Number of Gen 0 GCs / sec           | 71.00         |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 6.00          |
| Max Gen 0 Size (B)                      | 8,134,776     |
| Max Gen 1 Size (B)                      | 12,517,832    |
| Max Gen 2 Size (B)                      | 5,711,664     |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,635,208     |
| Max Allocation Rate (B/sec)             | 6,170,854,936 |
| Max GC Heap Fragmentation               | 26            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 26            |
| Max ThreadPool Threads Count            | 40            |
| Max ThreadPool Queue Length             | 76            |
| Max ThreadPool Items (#/s)              | 63,132        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 285,554       |
| Methods Jitted                          | 3,426         |


| load                   |                |
| ---------------------- | -------------- |
| CPU Usage (%)          | 28             |
| Cores usage (%)        | 341            |
| Working Set (MB)       | 52             |
| Private Memory (MB)    | 148            |
| Build Time (ms)        | 6,424          |
| Start Time (ms)        | 86             |
| Published Size (KB)    | 77,375         |
| .NET Core SDK Version  | 3.1.418        |
| ASP.NET Core Version   | 3.1.24+d1fa2cb |
| .NET Runtime Version   | 3.1.24+3b38386 |
| First Request (ms)     | 118            |
| Requests               | 786,545        |
| Bad responses          | 0              |
| Latency 50th (us)      | 4,852          |
| Latency 75th (us)      | 5,393          |
| Latency 90th (us)      | 5,860          |
| Latency 95th (us)      | 6,132          |
| Latency 99th (us)      | 6,806          |
| Mean latency (us)      | 4,877          |
| Max latency (us)       | 189,311        |
| Requests/sec           | 26,227         |
| Requests/sec (max)     | 33,860         |
| Read throughput (MB/s) | 361.64         |
```

### Utf8JsonWriter-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=Utf8JsonWriter-ArrayPool-NoValidation --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,198         |
| Working Set (MB)                        | 162           |
| Private Memory (MB)                     | 321           |
| Build Time (ms)                         | 2,508         |
| Start Time (ms)                         | 268           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 99            |
| Max Working Set (MB)                    | 169           |
| Max GC Heap Size (MB)                   | 89            |
| Size of committed memory by the GC (MB) | 111           |
| Max Number of Gen 0 GCs / sec           | 78.00         |
| Max Number of Gen 1 GCs / sec           | 14.00         |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 10.00         |
| Max Gen 0 Size (B)                      | 1,312,280     |
| Max Gen 1 Size (B)                      | 3,544,768     |
| Max Gen 2 Size (B)                      | 7,876,992     |
| Max LOH Size (B)                        | 192,544       |
| Max POH Size (B)                        | 1,672,288     |
| Max Allocation Rate (B/sec)             | 6,715,475,888 |
| Max GC Heap Fragmentation               | 42            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 7             |
| Max ThreadPool Threads Count            | 36            |
| Max ThreadPool Queue Length             | 81            |
| Max ThreadPool Items (#/s)              | 73,711        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 288,333       |
| Methods Jitted                          | 3,392         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 33      |
| Cores usage (%)        | 394     |
| Working Set (MB)       | 42      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 89      |
| First Request (ms)     | 115     |
| Requests               | 959,890 |
| Bad responses          | 0       |
| Latency 50th (us)      | 3,864   |
| Latency 75th (us)      | 4,290   |
| Latency 90th (us)      | 4,678   |
| Latency 95th (us)      | 4,984   |
| Latency 99th (us)      | 5,749   |
| Mean latency (us)      | 3,994   |
| Max latency (us)       | 146,004 |
| Requests/sec           | 32,007  |
| Requests/sec (max)     | 41,150  |
| Read throughput (MB/s) | 450.58  |
```

### ODataJsonWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Utf8-Async --application.options.counterProviders System.Runtime
```
```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,201         |
| Working Set (MB)                        | 277           |
| Private Memory (MB)                     | 292           |
| Build Time (ms)                         | 1,726         |
| Start Time (ms)                         | 264           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 107           |
| Max Working Set (MB)                    | 273           |
| Max GC Heap Size (MB)                   | 110           |
| Size of committed memory by the GC (MB) | 232           |
| Max Number of Gen 0 GCs / sec           | 43.00         |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 0.00          |
| Max Time in GC (%)                      | 5.00          |
| Max Gen 0 Size (B)                      | 2,505,864     |
| Max Gen 1 Size (B)                      | 11,436,112    |
| Max Gen 2 Size (B)                      | 5,467,360     |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,614,608     |
| Max Allocation Rate (B/sec)             | 3,726,757,440 |
| Max GC Heap Fragmentation               | 13            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 12            |
| Max ThreadPool Threads Count            | 31            |
| Max ThreadPool Queue Length             | 65            |
| Max ThreadPool Items (#/s)              | 1,548,525     |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 321,048       |
| Methods Jitted                          | 3,821         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 15      |
| Cores usage (%)        | 177     |
| Working Set (MB)       | 41      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 84      |
| First Request (ms)     | 130     |
| Requests               | 269,466 |
| Bad responses          | 0       |
| Latency 50th (us)      | 14,316  |
| Latency 75th (us)      | 14,789  |
| Latency 90th (us)      | 15,065  |
| Latency 95th (us)      | 15,369  |
| Latency 99th (us)      | 16,273  |
| Mean latency (us)      | 14,246  |
| Max latency (us)       | 184,320 |
| Requests/sec           | 8,983   |
| Requests/sec (max)     | 17,492  |
| Read throughput (MB/s) | 123.87  |
```

### ODataJsonWriter-Direct

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Direct --application.options.counterProviders System.Runtime
```
```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 98            |
| Cores usage (%)                         | 1,180         |
| Working Set (MB)                        | 267           |
| Private Memory (MB)                     | 323           |
| Build Time (ms)                         | 1,719         |
| Start Time (ms)                         | 269           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 100           |
| Max Working Set (MB)                    | 261           |
| Max GC Heap Size (MB)                   | 204           |
| Size of committed memory by the GC (MB) | 142           |
| Max Number of Gen 0 GCs / sec           | 38.00         |
| Max Number of Gen 1 GCs / sec           | 7.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 2.00          |
| Max Gen 0 Size (B)                      | 15,088,728    |
| Max Gen 1 Size (B)                      | 11,588,088    |
| Max Gen 2 Size (B)                      | 9,591,808     |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,709,512     |
| Max Allocation Rate (B/sec)             | 3,328,632,600 |
| Max GC Heap Fragmentation               | 59            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 15            |
| Max ThreadPool Threads Count            | 32            |
| Max ThreadPool Queue Length             | 78            |
| Max ThreadPool Items (#/s)              | 94,999        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 279,220       |
| Methods Jitted                          | 3,322         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 39        |
| Cores usage (%)        | 468       |
| Working Set (MB)       | 41        |
| Private Memory (MB)    | 110       |
| Start Time (ms)        | 89        |
| First Request (ms)     | 116       |
| Requests               | 1,189,732 |
| Bad responses          | 0         |
| Latency 50th (us)      | 3,167     |
| Latency 75th (us)      | 3,415     |
| Latency 90th (us)      | 3,776     |
| Latency 95th (us)      | 3,996     |
| Latency 99th (us)      | 4,464     |
| Mean latency (us)      | 3,222     |
| Max latency (us)       | 159,565   |
| Requests/sec           | 39,689    |
| Requests/sec (max)     | 47,517    |
| Read throughput (MB/s) | 547.04    |
```

### Utf8JsonWriter-Direct-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=Utf8JsonWriter-Direct-ArrayPool-NoValidation --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 97            |
| Cores usage (%)                         | 1,167         |
| Working Set (MB)                        | 161           |
| Private Memory (MB)                     | 321           |
| Build Time (ms)                         | 1,702         |
| Start Time (ms)                         | 262           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 98            |
| Max Working Set (MB)                    | 168           |
| Max GC Heap Size (MB)                   | 85            |
| Size of committed memory by the GC (MB) | 108           |
| Max Number of Gen 0 GCs / sec           | 43.00         |
| Max Number of Gen 1 GCs / sec           | 9.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 3.00          |
| Max Gen 0 Size (B)                      | 14,526,392    |
| Max Gen 1 Size (B)                      | 3,663,344     |
| Max Gen 2 Size (B)                      | 7,374,320     |
| Max LOH Size (B)                        | 316,976       |
| Max POH Size (B)                        | 1,515,728     |
| Max Allocation Rate (B/sec)             | 3,689,479,016 |
| Max GC Heap Fragmentation               | 66            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 8             |
| Max ThreadPool Threads Count            | 34            |
| Max ThreadPool Queue Length             | 84            |
| Max ThreadPool Items (#/s)              | 141,313       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 283,031       |
| Methods Jitted                          | 3,285         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 43        |
| Cores usage (%)        | 518       |
| Working Set (MB)       | 42        |
| Private Memory (MB)    | 110       |
| Start Time (ms)        | 97        |
| First Request (ms)     | 116       |
| Requests               | 1,678,664 |
| Bad responses          | 0         |
| Latency 50th (us)      | 2,234     |
| Latency 75th (us)      | 2,427     |
| Latency 90th (us)      | 2,695     |
| Latency 95th (us)      | 2,948     |
| Latency 99th (us)      | 3,325     |
| Mean latency (us)      | 2,283     |
| Max latency (us)       | 276,915   |
| Requests/sec           | 55,984    |
| Requests/sec (max)     | 69,120    |
| Read throughput (MB/s) | 787.99    |
```

### ODataJsonWriter-Direct-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Direct-Async --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 1,196         |
| Working Set (MB)                        | 207           |
| Private Memory (MB)                     | 336           |
| Build Time (ms)                         | 1,696         |
| Start Time (ms)                         | 260           |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 102           |
| Max Working Set (MB)                    | 257           |
| Max GC Heap Size (MB)                   | 85            |
| Size of committed memory by the GC (MB) | 214           |
| Max Number of Gen 0 GCs / sec           | 33.00         |
| Max Number of Gen 1 GCs / sec           | 6.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 2.00          |
| Max Gen 0 Size (B)                      | 12,882,288    |
| Max Gen 1 Size (B)                      | 11,372,160    |
| Max Gen 2 Size (B)                      | 10,127,728    |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,334,520     |
| Max Allocation Rate (B/sec)             | 2,915,923,008 |
| Max GC Heap Fragmentation               | 66            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 15            |
| Max ThreadPool Threads Count            | 34            |
| Max ThreadPool Queue Length             | 62            |
| Max ThreadPool Items (#/s)              | 2,313,874     |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 313,504       |
| Methods Jitted                          | 3,672         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 18      |
| Cores usage (%)        | 221     |
| Working Set (MB)       | 42      |
| Private Memory (MB)    | 119     |
| Start Time (ms)        | 92      |
| First Request (ms)     | 127     |
| Requests               | 378,460 |
| Bad responses          | 0       |
| Latency 50th (us)      | 9,964   |
| Latency 75th (us)      | 10,362  |
| Latency 90th (us)      | 10,703  |
| Latency 95th (us)      | 10,831  |
| Latency 99th (us)      | 11,283  |
| Mean latency (us)      | 10,141  |
| Max latency (us)       | 200,848 |
| Requests/sec           | 12,625  |
| Requests/sec (max)     | 27,055  |
| Read throughput (MB/s) | 173.98  |
```

