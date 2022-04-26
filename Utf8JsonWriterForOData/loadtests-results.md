# Loadtests results

## Results summary

Results on lab machines (128 connections):

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

Results on Azure server (128 connections)

| Writer | Total Requests|Avg latency (ms)|Avg. RPS|CPU Usage (% normalized)
---------|---------------|----------------|--------|-------------------------
ODataMessageWriter-Utf8|17,623|218|584|60
ODataMessageWriter-NoOp|15,412|250|516|90
ODataMessageWriter-Utf8-Async|17,079|225|565|51
ODataJsonWriter-Utf8|16,035|240|531|19
Utf8JsonWriter-ArrayPool-NoValidation|16,407|235|543|56
ODataJsonWriter-Utf8-Async|17,649|218|584|16
ODataJsonWriter-Direct|16,907|228|560|17
Utf8JsonWriter-Direct-ArrayPool-NoValidation|17,380|222|575|16
ODataJsonWriter-Direct-Async|17,634|218|584|53

Results on Azure server (512 connections)

| Writer|Total Requests|Avg latency (ms)|Avg. RPS|CPU Usage (% normalized)
------|--------------|----------------|--------|-------------------------
ODataMessageWriter-Utf8|41,006|379|1,364|98
ODataMessageWriter-NoOp|54,836|281|1,808|100
ODataMessageWriter-Utf8-Async|21,189|734|691|100
ODataJsonWriter-Utf8|39,459|392|1,322|57
Utf8JsonWriter-ArrayPool-NoValidation|40,029|387|1,363|43
ODataJsonWriter-Utf8-Async|29,252|527|995|98
ODataJsonWriter-Direct|42,075|368|1,399|55
Utf8JsonWriter-Direct-ArrayPool-NoValidation|46,015|337|1,527|30
ODataJsonWriter-Direct-Async|35,807|236|1,221|85

## Raw results on lab machines
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

## Raw Results on Azure

### ODataMessageWriter-Utf8

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-Utf8 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 60            |
| Cores usage (%)                         | 241           |
| Working Set (MB)                        | 234           |
| Private Memory (MB)                     | 353           |
| Build Time (ms)                         | 22,141        |
| Start Time (ms)                         | 3,138         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 66            |
| Max Working Set (MB)                    | 260           |
| Max GC Heap Size (MB)                   | 202           |
| Size of committed memory by the GC (MB) | 169           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 3.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 1.00          |
| Max Gen 0 Size (B)                      | 262,560       |
| Max Gen 1 Size (B)                      | 10,925,504    |
| Max Gen 2 Size (B)                      | 7,118,504     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 1,519,008     |
| Max Allocation Rate (B/sec)             | 375,593,280   |
| Max GC Heap Fragmentation               | 1             |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 386           |
| Max Lock Contention (#/s)               | 17            |
| Max ThreadPool Threads Count            | 15            |
| Max ThreadPool Queue Length             | 8             |
| Max ThreadPool Items (#/s)              | 6,543         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 383,047       |
| Methods Jitted                          | 5,231         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 2         |
| Cores usage (%)        | 15        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 24        |
| Start Time (ms)        | 776       |
| First Request (ms)     | 908       |
| Requests               | 17,623    |
| Bad responses          | 0         |
| Latency 50th (us)      | 215,432   |
| Latency 75th (us)      | 216,806   |
| Latency 90th (us)      | 219,057   |
| Latency 95th (us)      | 222,000   |
| Latency 99th (us)      | 242,002   |
| Mean latency (us)      | 218,461   |
| Max latency (us)       | 1,170,588 |
| Requests/sec           | 584       |
| Requests/sec (max)     | 3,077     |
| Read throughput (MB/s) | 8.04      |
```

### ODataMessageWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-Utf8-Async --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 90            |
| Cores usage (%)                         | 360           |
| Working Set (MB)                        | 246           |
| Private Memory (MB)                     | 246           |
| Build Time (ms)                         | 7,256         |
| Start Time (ms)                         | 3,755         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 90            |
| Max Working Set (MB)                    | 269           |
| Max GC Heap Size (MB)                   | 210           |
| Size of committed memory by the GC (MB) | 198           |
| Max Number of Gen 0 GCs / sec           | 8.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 3.00          |
| Max Gen 0 Size (B)                      | 2,382,456     |
| Max Gen 1 Size (B)                      | 10,355,112    |
| Max Gen 2 Size (B)                      | 5,814,168     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 2,062,792     |
| Max Allocation Rate (B/sec)             | 598,264,960   |
| Max GC Heap Fragmentation               | 1             |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 387           |
| Max Lock Contention (#/s)               | 10            |
| Max ThreadPool Threads Count            | 16            |
| Max ThreadPool Queue Length             | 25            |
| Max ThreadPool Items (#/s)              | 98,698        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 489,416       |
| Methods Jitted                          | 6,279         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 3       |
| Cores usage (%)        | 28      |
| Working Set (MB)       | 28      |
| Private Memory (MB)    | 25      |
| Start Time (ms)        | 82      |
| First Request (ms)     | 924     |
| Requests               | 15,412  |
| Bad responses          | 0       |
| Latency 50th (us)      | 242,645 |
| Latency 75th (us)      | 257,455 |
| Latency 90th (us)      | 277,037 |
| Latency 95th (us)      | 291,267 |
| Latency 99th (us)      | 380,360 |
| Mean latency (us)      | 249,953 |
| Max latency (us)       | 648,120 |
| Requests/sec           | 516     |
| Requests/sec (max)     | 5,346   |
| Read throughput (MB/s) | 6.99    |
```

### ODataMessageWriter-NoOp

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-NoOp --application.options.counterProviders System.Runtime
```
```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 51            |
| Cores usage (%)                         | 202           |
| Working Set (MB)                        | 154           |
| Private Memory (MB)                     | 300           |
| Build Time (ms)                         | 23,679        |
| Start Time (ms)                         | 3,402         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 57            |
| Max Working Set (MB)                    | 160           |
| Max GC Heap Size (MB)                   | 89            |
| Size of committed memory by the GC (MB) | 101           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 2.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 3,149,456     |
| Max Gen 2 Size (B)                      | 4,707,904     |
| Max LOH Size (B)                        | 707,088       |
| Max POH Size (B)                        | 603,960       |
| Max Allocation Rate (B/sec)             | 358,669,488   |
| Max GC Heap Fragmentation               | 27            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 387           |
| Max Lock Contention (#/s)               | 12            |
| Max ThreadPool Threads Count            | 14            |
| Max ThreadPool Queue Length             | 1             |
| Max ThreadPool Items (#/s)              | 2,256         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 382,115       |
| Methods Jitted                          | 5,265         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 5       |
| Cores usage (%)        | 38      |
| Working Set (MB)       | 28      |
| Private Memory (MB)    | 24      |
| Start Time (ms)        | 87      |
| First Request (ms)     | 786     |
| Requests               | 17,079  |
| Bad responses          | 0       |
| Latency 50th (us)      | 221,001 |
| Latency 75th (us)      | 227,023 |
| Latency 90th (us)      | 234,377 |
| Latency 95th (us)      | 240,851 |
| Latency 99th (us)      | 280,145 |
| Mean latency (us)      | 225,482 |
| Max latency (us)       | 522,134 |
| Requests/sec           | 565     |
| Requests/sec (max)     | 2,369   |
| Read throughput (MB/s) | 0.07    |
```

### ODataJsonWriter-Utf8

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Utf8 --application.options.counterProviders System.Runtime
```
```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 19            |
| Cores usage (%)                         | 76            |
| Working Set (MB)                        | 265           |
| Private Memory (MB)                     | 332           |
| Build Time (ms)                         | 7,157         |
| Start Time (ms)                         | 3,499         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 22            |
| Max Working Set (MB)                    | 283           |
| Max GC Heap Size (MB)                   | 127           |
| Size of committed memory by the GC (MB) | 234           |
| Max Number of Gen 0 GCs / sec           | 2.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 1.00          |
| Max Gen 0 Size (B)                      | 57,478,264    |
| Max Gen 1 Size (B)                      | 5,683,304     |
| Max Gen 2 Size (B)                      | 5,974,152     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 1,251,224     |
| Max Allocation Rate (B/sec)             | 135,149,056   |
| Max GC Heap Fragmentation               | 88            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 383           |
| Max Lock Contention (#/s)               | 31            |
| Max ThreadPool Threads Count            | 19            |
| Max ThreadPool Queue Length             | 1             |
| Max ThreadPool Items (#/s)              | 8,964         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 319,917       |
| Methods Jitted                          | 3,850         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 4         |
| Cores usage (%)        | 35        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 26        |
| Start Time (ms)        | 815       |
| First Request (ms)     | 854       |
| Requests               | 16,035    |
| Bad responses          | 0         |
| Latency 50th (us)      | 215,680   |
| Latency 75th (us)      | 219,001   |
| Latency 90th (us)      | 227,988   |
| Latency 95th (us)      | 240,301   |
| Latency 99th (us)      | 1,170,964 |
| Mean latency (us)      | 240,172   |
| Max latency (us)       | 2,693,818 |
| Requests/sec           | 531       |
| Requests/sec (max)     | 2,350     |
| Read throughput (MB/s) | 7.33      |
```

### ODataJsonWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Utf8-Async --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 56            |
| Cores usage (%)                         | 223           |
| Working Set (MB)                        | 233           |
| Private Memory (MB)                     | 354           |
| Build Time (ms)                         | 7,213         |
| Start Time (ms)                         | 3,430         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 61            |
| Max Working Set (MB)                    | 267           |
| Max GC Heap Size (MB)                   | 116           |
| Size of committed memory by the GC (MB) | 236           |
| Max Number of Gen 0 GCs / sec           | 4.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 10,980,976    |
| Max Gen 2 Size (B)                      | 7,042,736     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 847,448       |
| Max Allocation Rate (B/sec)             | 232,976,048   |
| Max GC Heap Fragmentation               | 1             |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 384           |
| Max Lock Contention (#/s)               | 23            |
| Max ThreadPool Threads Count            | 19            |
| Max ThreadPool Queue Length             | 4             |
| Max ThreadPool Items (#/s)              | 105,983       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 336,154       |
| Methods Jitted                          | 4,012         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 4         |
| Cores usage (%)        | 31        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 25        |
| Start Time (ms)        | 160       |
| First Request (ms)     | 756       |
| Requests               | 16,407    |
| Bad responses          | 0         |
| Latency 50th (us)      | 215,822   |
| Latency 75th (us)      | 218,922   |
| Latency 90th (us)      | 227,148   |
| Latency 95th (us)      | 240,001   |
| Latency 99th (us)      | 1,154,559 |
| Mean latency (us)      | 234,726   |
| Max latency (us)       | 2,437,335 |
| Requests/sec           | 543       |
| Requests/sec (max)     | 1,851     |
| Read throughput (MB/s) | 7.49      |
```

### Utf8JsonWriter-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=Utf8JsonWriter-ArrayPool-NoValidation --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 16            |
| Cores usage (%)                         | 62            |
| Working Set (MB)                        | 152           |
| Private Memory (MB)                     | 153           |
| Build Time (ms)                         | 10,529        |
| Start Time (ms)                         | 3,385         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 15            |
| Max Working Set (MB)                    | 159           |
| Max GC Heap Size (MB)                   | 96            |
| Size of committed memory by the GC (MB) | 103           |
| Max Number of Gen 0 GCs / sec           | 2.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 1.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 5,850,904     |
| Max Gen 2 Size (B)                      | 3,191,360     |
| Max LOH Size (B)                        | 313,760       |
| Max POH Size (B)                        | 1,193,472     |
| Max Allocation Rate (B/sec)             | 126,041,720   |
| Max GC Heap Fragmentation               | 1             |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 385           |
| Max Lock Contention (#/s)               | 19            |
| Max ThreadPool Threads Count            | 21            |
| Max ThreadPool Queue Length             | 1             |
| Max ThreadPool Items (#/s)              | 2,952         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 302,075       |
| Methods Jitted                          | 3,566         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 2         |
| Cores usage (%)        | 17        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 26        |
| Start Time (ms)        | 718       |
| First Request (ms)     | 1,029     |
| Requests               | 17,649    |
| Bad responses          | 0         |
| Latency 50th (us)      | 214,081   |
| Latency 75th (us)      | 214,837   |
| Latency 90th (us)      | 216,308   |
| Latency 95th (us)      | 217,898   |
| Latency 99th (us)      | 230,042   |
| Mean latency (us)      | 217,975   |
| Max latency (us)       | 1,159,713 |
| Requests/sec           | 584       |
| Requests/sec (max)     | 5,200     |
| Read throughput (MB/s) | 8.25      |
```

### ODataJsonWriter-Direct

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Direct --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 17            |
| Cores usage (%)                         | 68            |
| Working Set (MB)                        | 245           |
| Private Memory (MB)                     | 245           |
| Build Time (ms)                         | 7,159         |
| Start Time (ms)                         | 3,236         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 16            |
| Max Working Set (MB)                    | 255           |
| Max GC Heap Size (MB)                   | 187           |
| Size of committed memory by the GC (MB) | 216           |
| Max Number of Gen 0 GCs / sec           | 1.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 5,565,960     |
| Max Gen 2 Size (B)                      | 5,565,768     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 962,752       |
| Max Allocation Rate (B/sec)             | 49,964,944    |
| Max GC Heap Fragmentation               | 2             |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 387           |
| Max Lock Contention (#/s)               | 15            |
| Max ThreadPool Threads Count            | 17            |
| Max ThreadPool Queue Length             | 0             |
| Max ThreadPool Items (#/s)              | 7,009         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 293,067       |
| Methods Jitted                          | 3,491         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 3         |
| Cores usage (%)        | 28        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 25        |
| Start Time (ms)        | 86        |
| First Request (ms)     | 639       |
| Requests               | 16,907    |
| Bad responses          | 0         |
| Latency 50th (us)      | 214,651   |
| Latency 75th (us)      | 215,657   |
| Latency 90th (us)      | 218,267   |
| Latency 95th (us)      | 223,441   |
| Latency 99th (us)      | 936,244   |
| Mean latency (us)      | 227,847   |
| Max latency (us)       | 1,405,658 |
| Requests/sec           | 560       |
| Requests/sec (max)     | 4,350     |
| Read throughput (MB/s) | 7.72      |
```

### ODataJsonWriter-Direct-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Direct-Async --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 53            |
| Cores usage (%)                         | 212           |
| Working Set (MB)                        | 254           |
| Private Memory (MB)                     | 350           |
| Build Time (ms)                         | 9,844         |
| Start Time (ms)                         | 3,682         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 52            |
| Max Working Set (MB)                    | 265           |
| Max GC Heap Size (MB)                   | 137           |
| Size of committed memory by the GC (MB) | 226           |
| Max Number of Gen 0 GCs / sec           | 3.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 5,978,336     |
| Max Gen 1 Size (B)                      | 5,739,624     |
| Max Gen 2 Size (B)                      | 6,996,448     |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 1,448,912     |
| Max Allocation Rate (B/sec)             | 122,787,568   |
| Max GC Heap Fragmentation               | 30            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 385           |
| Max Lock Contention (#/s)               | 18            |
| Max ThreadPool Threads Count            | 18            |
| Max ThreadPool Queue Length             | 4             |
| Max ThreadPool Items (#/s)              | 105,857       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 325,615       |
| Methods Jitted                          | 3,857         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 3         |
| Cores usage (%)        | 23        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 23        |
| Start Time (ms)        | 761       |
| First Request (ms)     | 832       |
| Requests               | 17,634    |
| Bad responses          | 0         |
| Latency 50th (us)      | 214,879   |
| Latency 75th (us)      | 215,996   |
| Latency 90th (us)      | 217,975   |
| Latency 95th (us)      | 220,682   |
| Latency 99th (us)      | 241,076   |
| Mean latency (us)      | 218,337   |
| Max latency (us)       | 1,446,716 |
| Requests/sec           | 584       |
| Requests/sec (max)     | 3,382     |
| Read throughput (MB/s) | 8.06      |
```

### Utf8JsonWriter-Direct-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=Utf8JsonWriter-Direct-ArrayPool-NoValidation --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 16            |
| Cores usage (%)                         | 65            |
| Working Set (MB)                        | 148           |
| Private Memory (MB)                     | 150           |
| Build Time (ms)                         | 9,767         |
| Start Time (ms)                         | 3,417         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 13            |
| Max Working Set (MB)                    | 154           |
| Max GC Heap Size (MB)                   | 87            |
| Size of committed memory by the GC (MB) | 99            |
| Max Number of Gen 0 GCs / sec           | 1.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 3,329,056     |
| Max Gen 2 Size (B)                      | 3,186,608     |
| Max LOH Size (B)                        | 313,760       |
| Max POH Size (B)                        | 942,152       |
| Max Allocation Rate (B/sec)             | 36,275,560    |
| Max GC Heap Fragmentation               | 3             |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 385           |
| Max Lock Contention (#/s)               | 12            |
| Max ThreadPool Threads Count            | 24            |
| Max ThreadPool Queue Length             | 1             |
| Max ThreadPool Items (#/s)              | 2,440         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 297,552       |
| Methods Jitted                          | 3,472         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 2         |
| Cores usage (%)        | 14        |
| Working Set (MB)       | 28        |
| Private Memory (MB)    | 25        |
| Start Time (ms)        | 713       |
| First Request (ms)     | 1,027     |
| Requests               | 17,380    |
| Bad responses          | 0         |
| Latency 50th (us)      | 214,213   |
| Latency 75th (us)      | 215,185   |
| Latency 90th (us)      | 218,031   |
| Latency 95th (us)      | 221,327   |
| Latency 99th (us)      | 643,532   |
| Mean latency (us)      | 221,727   |
| Max latency (us)       | 1,282,341 |
| Requests/sec           | 575       |
| Requests/sec (max)     | 4,991     |
| Read throughput (MB/s) | 8.10      |
```

## Raw Results on Azure with 512 connections

### ODataMessageWriter-Utf8

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-Utf8 --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 98            |
| Cores usage (%)                         | 392           |
| Working Set (MB)                        | 310           |
| Private Memory (MB)                     | 453           |
| Build Time (ms)                         | 10,250        |
| Start Time (ms)                         | 3,627         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 102           |
| Max Working Set (MB)                    | 324           |
| Max GC Heap Size (MB)                   | 248           |
| Size of committed memory by the GC (MB) | 264           |
| Max Number of Gen 0 GCs / sec           | 12.00         |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 7.00          |
| Max Gen 0 Size (B)                      | 6,069,696     |
| Max Gen 1 Size (B)                      | 10,340,336    |
| Max Gen 2 Size (B)                      | 10,000,136    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 5,898,568     |
| Max Allocation Rate (B/sec)             | 1,082,396,712 |
| Max GC Heap Fragmentation               | 1             |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,536         |
| Max Lock Contention (#/s)               | 27            |
| Max ThreadPool Threads Count            | 16            |
| Max ThreadPool Queue Length             | 169           |
| Max ThreadPool Items (#/s)              | 6,732         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 383,584       |
| Methods Jitted                          | 5,240         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 10        |
| Cores usage (%)        | 83        |
| Working Set (MB)       | 32        |
| Private Memory (MB)    | 39        |
| Start Time (ms)        | 773       |
| First Request (ms)     | 957       |
| Requests               | 41,006    |
| Bad responses          | 0         |
| Latency 50th (us)      | 259,001   |
| Latency 75th (us)      | 288,037   |
| Latency 90th (us)      | 696,594   |
| Latency 95th (us)      | 1,300,155 |
| Latency 99th (us)      | 1,932,243 |
| Mean latency (us)      | 379,462   |
| Max latency (us)       | 6,085,301 |
| Requests/sec           | 1,364     |
| Requests/sec (max)     | 7,086     |
| Read throughput (MB/s) | 17.77     |
```

### ODataMessageWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-Utf8-Async --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 401           |
| Working Set (MB)                        | 280           |
| Private Memory (MB)                     | 417           |
| Build Time (ms)                         | 7,219         |
| Start Time (ms)                         | 3,265         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 109           |
| Max Working Set (MB)                    | 286           |
| Max GC Heap Size (MB)                   | 215           |
| Size of committed memory by the GC (MB) | 225           |
| Max Number of Gen 0 GCs / sec           | 10.00         |
| Max Number of Gen 1 GCs / sec           | 4.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 5.00          |
| Max Gen 0 Size (B)                      | 39,233,456    |
| Max Gen 1 Size (B)                      | 7,679,856     |
| Max Gen 2 Size (B)                      | 17,858,160    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 6,425,872     |
| Max Allocation Rate (B/sec)             | 865,829,432   |
| Max GC Heap Fragmentation               | 35            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,500         |
| Max Lock Contention (#/s)               | 44            |
| Max ThreadPool Threads Count            | 13            |
| Max ThreadPool Queue Length             | 266           |
| Max ThreadPool Items (#/s)              | 140,835       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 487,783       |
| Methods Jitted                          | 6,252         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 4         |
| Cores usage (%)        | 29        |
| Working Set (MB)       | 30        |
| Private Memory (MB)    | 38        |
| Start Time (ms)        | 81        |
| First Request (ms)     | 852       |
| Requests               | 21,189    |
| Bad responses          | 0         |
| Latency 50th (us)      | 590,999   |
| Latency 75th (us)      | 733,810   |
| Latency 90th (us)      | 1,467,470 |
| Latency 95th (us)      | 1,649,451 |
| Latency 99th (us)      | 2,635,351 |
| Mean latency (us)      | 733,752   |
| Max latency (us)       | 5,168,956 |
| Requests/sec           | 691       |
| Requests/sec (max)     | 5,601     |
| Read throughput (MB/s) | 9.20      |
```

### ODataMessageWriter-NoOp

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataMessageWriter-NoOp --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 100           |
| Cores usage (%)                         | 401           |
| Working Set (MB)                        | 166           |
| Private Memory (MB)                     | 311           |
| Build Time (ms)                         | 9,869         |
| Start Time (ms)                         | 3,546         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 105           |
| Max Working Set (MB)                    | 173           |
| Max GC Heap Size (MB)                   | 101           |
| Size of committed memory by the GC (MB) | 113           |
| Max Number of Gen 0 GCs / sec           | 14.00         |
| Max Number of Gen 1 GCs / sec           | 4.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 9.00          |
| Max Gen 0 Size (B)                      | 96            |
| Max Gen 1 Size (B)                      | 3,276,968     |
| Max Gen 2 Size (B)                      | 14,316,192    |
| Max LOH Size (B)                        | 1,762,304     |
| Max POH Size (B)                        | 1,460,920     |
| Max Allocation Rate (B/sec)             | 1,256,530,632 |
| Max GC Heap Fragmentation               | 41            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,539         |
| Max Lock Contention (#/s)               | 26            |
| Max ThreadPool Threads Count            | 16            |
| Max ThreadPool Queue Length             | 124           |
| Max ThreadPool Items (#/s)              | 4,444         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 382,424       |
| Methods Jitted                          | 5,270         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 7       |
| Cores usage (%)        | 53      |
| Working Set (MB)       | 28      |
| Private Memory (MB)    | 38      |
| Start Time (ms)        | 735     |
| First Request (ms)     | 962     |
| Requests               | 54,836  |
| Bad responses          | 0       |
| Latency 50th (us)      | 269,995 |
| Latency 75th (us)      | 286,630 |
| Latency 90th (us)      | 320,260 |
| Latency 95th (us)      | 353,626 |
| Latency 99th (us)      | 462,999 |
| Mean latency (us)      | 280,845 |
| Max latency (us)       | 742,992 |
| Requests/sec           | 1,808   |
| Requests/sec (max)     | 8,701   |
| Read throughput (MB/s) | 0.24    |
```

### ODataJsonWriter-Utf8

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Utf8 --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 57            |
| Cores usage (%)                         | 228           |
| Working Set (MB)                        | 350           |
| Private Memory (MB)                     | 350           |
| Build Time (ms)                         | 9,958         |
| Start Time (ms)                         | 3,537         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 58            |
| Max Working Set (MB)                    | 366           |
| Max GC Heap Size (MB)                   | 200           |
| Size of committed memory by the GC (MB) | 317           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 3.00          |
| Max Gen 0 Size (B)                      | 18,628,472    |
| Max Gen 1 Size (B)                      | 8,892,976     |
| Max Gen 2 Size (B)                      | 18,824,296    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 2,169,984     |
| Max Allocation Rate (B/sec)             | 392,313,264   |
| Max GC Heap Fragmentation               | 60            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,476         |
| Max Lock Contention (#/s)               | 56            |
| Max ThreadPool Threads Count            | 18            |
| Max ThreadPool Queue Length             | 3             |
| Max ThreadPool Items (#/s)              | 21,633        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 298,590       |
| Methods Jitted                          | 3,596         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 8         |
| Cores usage (%)        | 67        |
| Working Set (MB)       | 38        |
| Private Memory (MB)    | 47        |
| Start Time (ms)        | 1,576     |
| First Request (ms)     | 1,248     |
| Requests               | 39,459    |
| Bad responses          | 0         |
| Latency 50th (us)      | 226,574   |
| Latency 75th (us)      | 245,054   |
| Latency 90th (us)      | 988,128   |
| Latency 95th (us)      | 1,275,009 |
| Latency 99th (us)      | 2,459,542 |
| Mean latency (us)      | 392,144   |
| Max latency (us)       | 7,304,602 |
| Requests/sec           | 1,322     |
| Requests/sec (max)     | 6,929     |
| Read throughput (MB/s) | 17.10     |
```

### ODataJsonWriter-Utf8-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Utf8-Async --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 98            |
| Cores usage (%)                         | 391           |
| Working Set (MB)                        | 201           |
| Private Memory (MB)                     | 333           |
| Build Time (ms)                         | 7,402         |
| Start Time (ms)                         | 3,353         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 90            |
| Max Working Set (MB)                    | 251           |
| Max GC Heap Size (MB)                   | 97            |
| Size of committed memory by the GC (MB) | 202           |
| Max Number of Gen 0 GCs / sec           | 6.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 3.00          |
| Max Gen 0 Size (B)                      | 2,189,080     |
| Max Gen 1 Size (B)                      | 3,147,440     |
| Max Gen 2 Size (B)                      | 18,503,912    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 4,345,416     |
| Max Allocation Rate (B/sec)             | 528,261,496   |
| Max GC Heap Fragmentation               | 44            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,497         |
| Max Lock Contention (#/s)               | 25            |
| Max ThreadPool Threads Count            | 19            |
| Max ThreadPool Queue Length             | 112           |
| Max ThreadPool Items (#/s)              | 218,907       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 336,462       |
| Methods Jitted                          | 4,024         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 9         |
| Cores usage (%)        | 75        |
| Working Set (MB)       | 36        |
| Private Memory (MB)    | 45        |
| Start Time (ms)        | 96        |
| First Request (ms)     | 707       |
| Requests               | 29,252    |
| Bad responses          | 0         |
| Latency 50th (us)      | 252,493   |
| Latency 75th (us)      | 481,331   |
| Latency 90th (us)      | 1,265,998 |
| Latency 95th (us)      | 1,728,232 |
| Latency 99th (us)      | 2,907,864 |
| Mean latency (us)      | 527,389   |
| Max latency (us)       | 9,416,318 |
| Requests/sec           | 995       |
| Requests/sec (max)     | 7,298     |
| Read throughput (MB/s) | 13.07     |
```

### Utf8JsonWriter-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=Utf8JsonWriter-ArrayPool-NoValidation --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 43            |
| Cores usage (%)                         | 172           |
| Working Set (MB)                        | 254           |
| Private Memory (MB)                     | 357           |
| Build Time (ms)                         | 7,014         |
| Start Time (ms)                         | 3,278         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 36            |
| Max Working Set (MB)                    | 230           |
| Max GC Heap Size (MB)                   | 142           |
| Size of committed memory by the GC (MB) | 173           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 3.00          |
| Max Gen 0 Size (B)                      | 773,472       |
| Max Gen 1 Size (B)                      | 6,107,872     |
| Max Gen 2 Size (B)                      | 13,885,680    |
| Max LOH Size (B)                        | 313,760       |
| Max POH Size (B)                        | 5,016,832     |
| Max Allocation Rate (B/sec)             | 367,594,512   |
| Max GC Heap Fragmentation               | 37            |
| # of Assemblies Loaded                  | 116           |
| Max Exceptions (#/s)                    | 1,461         |
| Max Lock Contention (#/s)               | 38            |
| Max ThreadPool Threads Count            | 18            |
| Max ThreadPool Queue Length             | 2             |
| Max ThreadPool Items (#/s)              | 8,657         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 302,339       |
| Methods Jitted                          | 3,570         |


| load                   |            |
| ---------------------- | ---------- |
| CPU Usage (%)          | 10         |
| Cores usage (%)        | 82         |
| Working Set (MB)       | 33         |
| Private Memory (MB)    | 42         |
| Start Time (ms)        | 869        |
| First Request (ms)     | 1,047      |
| Requests               | 40,029     |
| Bad responses          | 0          |
| Latency 50th (us)      | 225,998    |
| Latency 75th (us)      | 276,996    |
| Latency 90th (us)      | 845,959    |
| Latency 95th (us)      | 1,212,549  |
| Latency 99th (us)      | 2,336,965  |
| Mean latency (us)      | 387,833    |
| Max latency (us)       | 11,306,599 |
| Requests/sec           | 1,363      |
| Requests/sec (max)     | 5,652      |
| Read throughput (MB/s) | 17.76      |
```

### ODataJsonWriter-Direct

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Direct --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 55            |
| Cores usage (%)                         | 220           |
| Working Set (MB)                        | 327           |
| Private Memory (MB)                     | 349           |
| Build Time (ms)                         | 7,246         |
| Start Time (ms)                         | 3,337         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 53            |
| Max Working Set (MB)                    | 342           |
| Max GC Heap Size (MB)                   | 267           |
| Size of committed memory by the GC (MB) | 289           |
| Max Number of Gen 0 GCs / sec           | 2.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 1.00          |
| Max Gen 0 Size (B)                      | 23,110,208    |
| Max Gen 1 Size (B)                      | 9,218,096     |
| Max Gen 2 Size (B)                      | 15,423,600    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 2,334,712     |
| Max Allocation Rate (B/sec)             | 149,297,912   |
| Max GC Heap Fragmentation               | 71            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 1,536         |
| Max Lock Contention (#/s)               | 58            |
| Max ThreadPool Threads Count            | 14            |
| Max ThreadPool Queue Length             | 2             |
| Max ThreadPool Items (#/s)              | 19,114        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 293,717       |
| Methods Jitted                          | 3,508         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 7         |
| Cores usage (%)        | 54        |
| Working Set (MB)       | 36        |
| Private Memory (MB)    | 44        |
| Start Time (ms)        | 131       |
| First Request (ms)     | 673       |
| Requests               | 42,075    |
| Bad responses          | 0         |
| Latency 50th (us)      | 220,698   |
| Latency 75th (us)      | 235,003   |
| Latency 90th (us)      | 949,883   |
| Latency 95th (us)      | 1,192,467 |
| Latency 99th (us)      | 1,762,098 |
| Mean latency (us)      | 368,516   |
| Max latency (us)       | 6,097,004 |
| Requests/sec           | 1,399     |
| Requests/sec (max)     | 5,845     |
| Read throughput (MB/s) | 18.45     |
```

### ODataJsonWriter-Direct-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=ODataJsonWriter-Direct-Async --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 85            |
| Cores usage (%)                         | 342           |
| Working Set (MB)                        | 384           |
| Private Memory (MB)                     | 458           |
| Build Time (ms)                         | 7,195         |
| Start Time (ms)                         | 3,329         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 87            |
| Max Working Set (MB)                    | 402           |
| Max GC Heap Size (MB)                   | 214           |
| Size of committed memory by the GC (MB) | 351           |
| Max Number of Gen 0 GCs / sec           | 4.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 2.00          |
| Max Gen 0 Size (B)                      | 18,120,240    |
| Max Gen 1 Size (B)                      | 9,143,920     |
| Max Gen 2 Size (B)                      | 15,950,968    |
| Max LOH Size (B)                        | 838,104       |
| Max POH Size (B)                        | 2,235,960     |
| Max Allocation Rate (B/sec)             | 343,143,888   |
| Max GC Heap Fragmentation               | 57            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 1,467         |
| Max Lock Contention (#/s)               | 50            |
| Max ThreadPool Threads Count            | 14            |
| Max ThreadPool Queue Length             | 137           |
| Max ThreadPool Items (#/s)              | 288,145       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 323,751       |
| Methods Jitted                          | 3,836         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 11        |
| Cores usage (%)        | 88        |
| Working Set (MB)       | 38        |
| Private Memory (MB)    | 47        |
| Start Time (ms)        | 469       |
| First Request (ms)     | 869       |
| Requests               | 35,807    |
| Bad responses          | 0         |
| Latency 50th (us)      | 236,021   |
| Latency 75th (us)      | 267,996   |
| Latency 90th (us)      | 1,159,890 |
| Latency 95th (us)      | 1,390,354 |
| Latency 99th (us)      | 2,537,986 |
| Mean latency (us)      | 435,379   |
| Max latency (us)       | 8,015,851 |
| Requests/sec           | 1,221     |
| Requests/sec (max)     | 6,223     |
| Read throughput (MB/s) | 15.39     |
```

### Utf8JsonWriter-Direct-ArrayPool-NoValidation

```
crank --config .\loadtests.yml --scenario mediumLoad --profile remote-win --variable writer=Utf8JsonWriter-Direct-ArrayPool-NoValidation --variable numConnections=512 --application.options.counterProviders System.Runtime
```

```
| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 30            |
| Cores usage (%)                         | 122           |
| Working Set (MB)                        | 282           |
| Private Memory (MB)                     | 339           |
| Build Time (ms)                         | 7,191         |
| Start Time (ms)                         | 3,198         |
| Published Size (KB)                     | 95,142        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 32            |
| Max Working Set (MB)                    | 280           |
| Max GC Heap Size (MB)                   | 219           |
| Size of committed memory by the GC (MB) | 249           |
| Max Number of Gen 0 GCs / sec           | 3.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 1.00          |
| Max Gen 0 Size (B)                      | 13,719,968    |
| Max Gen 1 Size (B)                      | 6,601,432     |
| Max Gen 2 Size (B)                      | 12,123,616    |
| Max LOH Size (B)                        | 313,760       |
| Max POH Size (B)                        | 2,886,792     |
| Max Allocation Rate (B/sec)             | 125,851,888   |
| Max GC Heap Fragmentation               | 58            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 1,533         |
| Max Lock Contention (#/s)               | 25            |
| Max ThreadPool Threads Count            | 15            |
| Max ThreadPool Queue Length             | 0             |
| Max ThreadPool Items (#/s)              | 7,472         |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 297,006       |
| Methods Jitted                          | 3,465         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 8         |
| Cores usage (%)        | 67        |
| Working Set (MB)       | 33        |
| Private Memory (MB)    | 40        |
| Start Time (ms)        | 89        |
| First Request (ms)     | 828       |
| Requests               | 46,015    |
| Bad responses          | 0         |
| Latency 50th (us)      | 222,999   |
| Latency 75th (us)      | 236,990   |
| Latency 90th (us)      | 510,003   |
| Latency 95th (us)      | 1,177,676 |
| Latency 99th (us)      | 1,690,435 |
| Mean latency (us)      | 336,729   |
| Max latency (us)       | 7,062,330 |
| Requests/sec           | 1,527     |
| Requests/sec (max)     | 6,381     |
| Read throughput (MB/s) | 20.62     |
```
