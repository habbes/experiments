# Loadtests results

## baseline
```
 crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=baseline --application.options.counterProviders System.Runtime
[04:48:44.339] Running session 'c627e8b0510245c1a33340f6a85518c1' with description ''
[04:48:45.098] Starting job 'application' ...
[04:48:45.403] Submitted job: http://asp-perf-win:5001/jobs/14
[04:48:46.052] 'application' has been selected by the server ...
[04:48:46.054] Using local folder: "C:\Users\clhabins\source\repos\experiments\Utf8JsonWriterForOData\src"
[04:49:16.272] Uploading C:\Users\clhabins\AppData\Local\Temp\tmpEC42.tmp (76,624KB)
[04:49:25.524] 'application' is now building ... http://asp-perf-win:5001/jobs/14/buildlog
[04:49:30.035] 'application' is running ... http://asp-perf-win:5001/jobs/14/output
[04:49:31.063] Starting job 'load' ...
[04:49:31.360] Submitted job: http://asp-perf-db:5001/jobs/9
[04:49:33.257] 'load' has been selected by the server ...
[04:49:33.551] 'load' is now building ... http://asp-perf-db:5001/jobs/9/buildlog
[04:50:20.242] Stopping job 'load' ...
[04:50:22.532] Deleting job 'load' ...
[04:50:23.132] Stopping job 'application' ...
[04:50:25.943] Deleting job 'application' ...

| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 99            |
| Cores usage (%)                         | 1,182         |
| Working Set (MB)                        | 158           |
| Private Memory (MB)                     | 319           |
| Build Time (ms)                         | 2,433         |
| Start Time (ms)                         | 260           |
| Published Size (KB)                     | 95,143        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 104           |
| Max Working Set (MB)                    | 165           |
| Max GC Heap Size (MB)                   | 92            |
| Size of committed memory by the GC (MB) | 106           |
| Max Number of Gen 0 GCs / sec           | 38.00         |
| Max Number of Gen 1 GCs / sec           | 7.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 10.00         |
| Max Gen 0 Size (B)                      | 15,456,144    |
| Max Gen 1 Size (B)                      | 7,725,200     |
| Max Gen 2 Size (B)                      | 6,453,296     |
| Max LOH Size (B)                        | 316,976       |
| Max POH Size (B)                        | 1,499,248     |
| Max Allocation Rate (B/sec)             | 3,147,361,992 |
| Max GC Heap Fragmentation               | 64            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 11            |
| Max ThreadPool Threads Count            | 33            |
| Max ThreadPool Queue Length             | 74            |
| Max ThreadPool Items (#/s)              | 128,177       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 241,070       |
| Methods Jitted                          | 2,827         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 41        |
| Cores usage (%)        | 493       |
| Working Set (MB)       | 41        |
| Private Memory (MB)    | 110       |
| Start Time (ms)        | 88        |
| First Request (ms)     | 110       |
| Requests               | 1,515,710 |
| Bad responses          | 0         |
| Latency 50th (us)      | 2,474     |
| Latency 75th (us)      | 2,638     |
| Latency 90th (us)      | 2,913     |
| Latency 95th (us)      | 3,163     |
| Latency 99th (us)      | 3,512     |
| Mean latency (us)      | 2,529     |
| Max latency (us)       | 255,530   |
| Requests/sec           | 50,527    |
| Requests/sec (max)     | 53,543    |
| Read throughput (MB/s) | 668.38    |
```

## JsonSerializer
```
 crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=baseline --application.options.counterProviders System.Runtime
[04:48:44.339] Running session 'c627e8b0510245c1a33340f6a85518c1' with description ''
[04:48:45.098] Starting job 'application' ...
[04:48:45.403] Submitted job: http://asp-perf-win:5001/jobs/14
[04:48:46.052] 'application' has been selected by the server ...
[04:48:46.054] Using local folder: "C:\Users\clhabins\source\repos\experiments\Utf8JsonWriterForOData\src"
[04:49:16.272] Uploading C:\Users\clhabins\AppData\Local\Temp\tmpEC42.tmp (76,624KB)
[04:49:25.524] 'application' is now building ... http://asp-perf-win:5001/jobs/14/buildlog
[04:49:30.035] 'application' is running ... http://asp-perf-win:5001/jobs/14/output
[04:49:31.063] Starting job 'load' ...
[04:49:31.360] Submitted job: http://asp-perf-db:5001/jobs/9
[04:49:33.257] 'load' has been selected by the server ...
[04:49:33.551] 'load' is now building ... http://asp-perf-db:5001/jobs/9/buildlog
[04:50:20.242] Stopping job 'load' ...
[04:50:22.532] Deleting job 'load' ...
[04:50:23.132] Stopping job 'application' ...
[04:50:25.943] Deleting job 'application' ...

| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 99            |
| Cores usage (%)                         | 1,182         |
| Working Set (MB)                        | 158           |
| Private Memory (MB)                     | 319           |
| Build Time (ms)                         | 2,433         |
| Start Time (ms)                         | 260           |
| Published Size (KB)                     | 95,143        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 104           |
| Max Working Set (MB)                    | 165           |
| Max GC Heap Size (MB)                   | 92            |
| Size of committed memory by the GC (MB) | 106           |
| Max Number of Gen 0 GCs / sec           | 38.00         |
| Max Number of Gen 1 GCs / sec           | 7.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 10.00         |
| Max Gen 0 Size (B)                      | 15,456,144    |
| Max Gen 1 Size (B)                      | 7,725,200     |
| Max Gen 2 Size (B)                      | 6,453,296     |
| Max LOH Size (B)                        | 316,976       |
| Max POH Size (B)                        | 1,499,248     |
| Max Allocation Rate (B/sec)             | 3,147,361,992 |
| Max GC Heap Fragmentation               | 64            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 11            |
| Max ThreadPool Threads Count            | 33            |
| Max ThreadPool Queue Length             | 74            |
| Max ThreadPool Items (#/s)              | 128,177       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 241,070       |
| Methods Jitted                          | 2,827         |


| load                   |           |
| ---------------------- | --------- |
| CPU Usage (%)          | 41        |
| Cores usage (%)        | 493       |
| Working Set (MB)       | 41        |
| Private Memory (MB)    | 110       |
| Start Time (ms)        | 88        |
| First Request (ms)     | 110       |
| Requests               | 1,515,710 |
| Bad responses          | 0         |
| Latency 50th (us)      | 2,474     |
| Latency 75th (us)      | 2,638     |
| Latency 90th (us)      | 2,913     |
| Latency 95th (us)      | 3,163     |
| Latency 99th (us)      | 3,512     |
| Mean latency (us)      | 2,529     |
| Max latency (us)       | 255,530   |
| Requests/sec           | 50,527    |
| Requests/sec (max)     | 53,543    |
| Read throughput (MB/s) | 668.38    |
```

## Utf8JsonWriter-Direct-ArrayPool-NoValidation

```
 crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=Utf8JsonWriter-Direct-ArrayPool-NoValidation --application.options.counterProviders System.Runtime
[04:58:32.366] Running session 'caffa9108e004aa3b76f37234439d0c6' with description ''
[04:58:33.052] Starting job 'application' ...
[04:58:33.356] Submitted job: http://asp-perf-win:5001/jobs/16
[04:58:35.312] 'application' has been selected by the server ...
[04:58:35.314] Using local folder: "C:\Users\clhabins\source\repos\experiments\Utf8JsonWriterForOData\src"
[04:58:43.941] Uploading C:\Users\clhabins\AppData\Local\Temp\tmpEA13.tmp (76,624KB)
[04:58:52.750] 'application' is now building ... http://asp-perf-win:5001/jobs/16/buildlog
[04:58:57.256] 'application' is running ... http://asp-perf-win:5001/jobs/16/output
[04:58:58.305] Starting job 'load' ...
[04:58:58.608] Submitted job: http://asp-perf-db:5001/jobs/11
[04:59:00.509] 'load' has been selected by the server ...
[04:59:00.808] 'load' is now building ... http://asp-perf-db:5001/jobs/11/buildlog
[04:59:01.401] 'load' is running ... http://asp-perf-db:5001/jobs/11/output
[04:59:47.458] Stopping job 'load' ...
[04:59:49.700] Deleting job 'load' ...
[04:59:50.294] Stopping job 'application' ...
[04:59:53.415] Deleting job 'application' ...

| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 18            |
| Cores usage (%)                         | 217           |
| Working Set (MB)                        | 157           |
| Private Memory (MB)                     | 317           |
| Build Time (ms)                         | 1,635         |
| Start Time (ms)                         | 259           |
| Published Size (KB)                     | 95,143        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 18            |
| Max Working Set (MB)                    | 163           |
| Max GC Heap Size (MB)                   | 87            |
| Size of committed memory by the GC (MB) | 104           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 1.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 7,859,536     |
| Max Gen 1 Size (B)                      | 9,457,368     |
| Max Gen 2 Size (B)                      | 4,873,288     |
| Max LOH Size (B)                        | 316,976       |
| Max POH Size (B)                        | 1,845,328     |
| Max Allocation Rate (B/sec)             | 282,570,080   |
| Max GC Heap Fragmentation               | 51            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 49            |
| Max ThreadPool Threads Count            | 41            |
| Max ThreadPool Queue Length             | 60            |
| Max ThreadPool Items (#/s)              | 11,028        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 293,835       |
| Methods Jitted                          | 3,431         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 7       |
| Cores usage (%)        | 85      |
| Working Set (MB)       | 43      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 88      |
| First Request (ms)     | 119     |
| Requests               | 139,985 |
| Bad responses          | 0       |
| Latency 50th (us)      | 28,111  |
| Latency 75th (us)      | 28,622  |
| Latency 90th (us)      | 29,109  |
| Latency 95th (us)      | 29,425  |
| Latency 99th (us)      | 30,279  |
| Mean latency (us)      | 27,431  |
| Max latency (us)       | 57,728  |
| Requests/sec           | 4,668   |
| Requests/sec (max)     | 15,718  |
| Read throughput (MB/s) | 65.68   |
```

## ODataJsonWriter-Direct

```
 crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Direct --application.options.counterProviders System.Runtime
[05:01:11.969] Running session 'c5ab321468544c168dfd076b5d25b8b0' with description ''
[05:01:12.652] Starting job 'application' ...
[05:01:12.956] Submitted job: http://asp-perf-win:5001/jobs/17
[05:01:14.899] 'application' has been selected by the server ...
[05:01:14.901] Using local folder: "C:\Users\clhabins\source\repos\experiments\Utf8JsonWriterForOData\src"
[05:01:22.746] Uploading C:\Users\clhabins\AppData\Local\Temp\tmp597D.tmp (76,624KB)
[05:01:36.440] 'application' is now building ... http://asp-perf-win:5001/jobs/17/buildlog
[05:01:40.960] 'application' is running ... http://asp-perf-win:5001/jobs/17/output
[05:01:42.004] Starting job 'load' ...
[05:01:42.306] Submitted job: http://asp-perf-db:5001/jobs/12
[05:01:44.224] 'load' has been selected by the server ...
[05:01:44.818] 'load' is now building ... http://asp-perf-db:5001/jobs/12/buildlog
[05:01:45.412] 'load' is running ... http://asp-perf-db:5001/jobs/12/output
[05:02:31.583] Stopping job 'load' ...
[05:02:33.835] Deleting job 'load' ...
[05:02:34.434] Stopping job 'application' ...
[05:02:37.242] Deleting job 'application' ...

| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 19            |
| Cores usage (%)                         | 231           |
| Working Set (MB)                        | 251           |
| Private Memory (MB)                     | 351           |
| Build Time (ms)                         | 1,639         |
| Start Time (ms)                         | 265           |
| Published Size (KB)                     | 95,143        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 19            |
| Max Working Set (MB)                    | 283           |
| Max GC Heap Size (MB)                   | 100           |
| Size of committed memory by the GC (MB) | 236           |
| Max Number of Gen 0 GCs / sec           | 5.00          |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 31,507,280    |
| Max Gen 1 Size (B)                      | 12,144,440    |
| Max Gen 2 Size (B)                      | 5,694,128     |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,326,208     |
| Max Allocation Rate (B/sec)             | 367,682,584   |
| Max GC Heap Fragmentation               | 58            |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 93            |
| Max ThreadPool Threads Count            | 43            |
| Max ThreadPool Queue Length             | 57            |
| Max ThreadPool Items (#/s)              | 11,454        |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 291,444       |
| Methods Jitted                          | 3,492         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 7       |
| Cores usage (%)        | 86      |
| Working Set (MB)       | 41      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 90      |
| First Request (ms)     | 117     |
| Requests               | 138,762 |
| Bad responses          | 0       |
| Latency 50th (us)      | 28,345  |
| Latency 75th (us)      | 29,026  |
| Latency 90th (us)      | 29,689  |
| Latency 95th (us)      | 30,108  |
| Latency 99th (us)      | 31,094  |
| Mean latency (us)      | 27,676  |
| Max latency (us)       | 44,076  |
| Requests/sec           | 4,626   |
| Requests/sec (max)     | 9,358   |
| Read throughput (MB/s) | 63.77   |
```

## ODataJsonWriter-Async

```
crank --config .\loadtests.yml --scenario mediumLoad --profile lab-win --variable writer=ODataJsonWriter-Direct-Async --application.options.counterProviders System.Runtime
[05:07:53.129] Running session '9935b52c10314a57a931650f910d3494' with description ''
[05:07:53.821] Starting job 'application' ...
[05:07:54.126] Submitted job: http://asp-perf-win:5001/jobs/18
[05:07:54.777] 'application' has been selected by the server ...
[05:07:54.779] Using local folder: "C:\Users\clhabins\source\repos\experiments\Utf8JsonWriterForOData\src"
[05:08:00.007] Uploading C:\Users\clhabins\AppData\Local\Temp\tmp7380.tmp (76,624KB)
[05:08:05.676] 'application' is now building ... http://asp-perf-win:5001/jobs/18/buildlog
[05:08:10.183] 'application' is running ... http://asp-perf-win:5001/jobs/18/output
[05:08:11.211] Starting job 'load' ...
[05:08:11.509] Submitted job: http://asp-perf-db:5001/jobs/13
[05:08:13.409] 'load' has been selected by the server ...
[05:08:13.706] 'load' is now building ... http://asp-perf-db:5001/jobs/13/buildlog
[05:08:15.599] 'load' is running ... http://asp-perf-db:5001/jobs/13/output
[05:09:01.633] Stopping job 'load' ...
[05:09:03.849] Deleting job 'load' ...
[05:09:04.443] Stopping job 'application' ...
[05:09:07.623] Deleting job 'application' ...

| application                             |               |
| --------------------------------------- | ------------- |
| CPU Usage (%)                           | 59            |
| Cores usage (%)                         | 709           |
| Working Set (MB)                        | 275           |
| Private Memory (MB)                     | 346           |
| Build Time (ms)                         | 1,649         |
| Start Time (ms)                         | 260           |
| Published Size (KB)                     | 95,143        |
| .NET Core SDK Version                   | 6.0.202       |
| ASP.NET Core Version                    | 6.0.4+f9ae0f5 |
| .NET Runtime Version                    | 6.0.4+be98e88 |
| Max CPU Usage (%)                       | 51            |
| Max Working Set (MB)                    | 218           |
| Max GC Heap Size (MB)                   | 159           |
| Size of committed memory by the GC (MB) | 153           |
| Max Number of Gen 0 GCs / sec           | 10.00         |
| Max Number of Gen 1 GCs / sec           | 2.00          |
| Max Number of Gen 2 GCs / sec           | 1.00          |
| Max Time in GC (%)                      | 0.00          |
| Max Gen 0 Size (B)                      | 1,495,344     |
| Max Gen 1 Size (B)                      | 15,112,872    |
| Max Gen 2 Size (B)                      | 7,073,784     |
| Max LOH Size (B)                        | 841,320       |
| Max POH Size (B)                        | 1,273,208     |
| Max Allocation Rate (B/sec)             | 781,178,544   |
| Max GC Heap Fragmentation               | 2             |
| # of Assemblies Loaded                  | 115           |
| Max Exceptions (#/s)                    | 0             |
| Max Lock Contention (#/s)               | 87            |
| Max ThreadPool Threads Count            | 36            |
| Max ThreadPool Queue Length             | 63            |
| Max ThreadPool Items (#/s)              | 612,116       |
| Max Active Timers                       | 0             |
| IL Jitted (B)                           | 320,759       |
| Methods Jitted                          | 3,805         |


| load                   |         |
| ---------------------- | ------- |
| CPU Usage (%)          | 6       |
| Cores usage (%)        | 78      |
| Working Set (MB)       | 41      |
| Private Memory (MB)    | 110     |
| Start Time (ms)        | 88      |
| First Request (ms)     | 127     |
| Requests               | 118,682 |
| Bad responses          | 0       |
| Latency 50th (us)      | 32,490  |
| Latency 75th (us)      | 33,182  |
| Latency 90th (us)      | 34,360  |
| Latency 95th (us)      | 35,127  |
| Latency 99th (us)      | 36,386  |
| Mean latency (us)      | 32,359  |
| Max latency (us)       | 55,675  |
| Requests/sec           | 3,953   |
| Requests/sec (max)     | 5,042   |
| Read throughput (MB/s) | 54.54   |
```