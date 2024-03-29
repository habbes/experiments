# Serializing large values with System.Text.Json lead to large memory allocations in LOH

This experiments demonstrates large allocations in LOH from `JsonSerializer` when serializing large strings or byte arrays. The issue occurs because `Utf8JsonWriter` buffers an entire value in memory before it can be written out to the output stream.

This issue is discussed in more detail in [this blog post](https://blog.habbes.xyz/cs-jsonserializer-memory-issues-when-serializing-large-string-and-byte-array-values) and [this GitHub issue](https://github.com/dotnet/runtime/issues/67337).

## Experiment

The experiment is a simple API with 3 endpoints that return JSON payloads with a single `Data` field.
- The `/string` endpoint returns a large string value (the source string contains 1,000,000 characters)
- The `/escaped-string` endpoint returns a large string value that contains characters that will require escaping
- The `/binary` endpoint returns a large base-64 string encoded from a source byte array with 1,000,000 bytes

The experiment consists of making 1,000 concurrent requests from 50 connections using [bombardier](https://github.com/codesenberg/bombardier) while using [Visual Studio's .NET Object Allocation Tracking profiler](https://learn.microsoft.com/en-us/visualstudio/profiling/dotnet-alloc-tool) to monitor heap allocations.

## Large byte arrays to base-64 encoding

```sh
.\bombardier.exe -l -n 1000 -c 50 http://localhost:5174/binary
```

```sh
Bombarding http://localhost:5174/binary with 1000 request(s) using 50 connection(s)
 1000 / 1000 [=======================================================================================] 100.00% 807/s 1s
Done!
Statistics        Avg      Stdev        Max
  Reqs/sec       895.44     407.18    2279.08
  Latency       56.62ms    29.88ms   209.70ms
  Latency Distribution
     50%    41.95ms
     75%    61.32ms
     90%    88.42ms
     95%   182.32ms
     99%   206.17ms
  HTTP codes:
    1xx - 0, 2xx - 1000, 3xx - 0, 4xx - 0, 5xx - 0
    others - 0
  Throughput:     1.07GB/s
```

![alt text](byte-array-allocations.png)

## Large string with no escaped characters

```sh
.\bombardier.exe -l -n 1000 -c 50 http://localhost:5174/string
```

```sh
Bombarding http://localhost:5174/string with 1000 request(s) using 50 connection(s)
 1000 / 1000 [======================================================================================] 100.00% 1380/s 0s
Done!
Statistics        Avg      Stdev        Max
  Reqs/sec      2066.66    1692.96    6846.19
  Latency       30.68ms    15.60ms   202.43ms
  Latency Distribution
     50%    22.20ms
     75%    25.28ms
     90%    61.26ms
     95%    89.86ms
     99%   143.50ms
  HTTP codes:
    1xx - 0, 2xx - 1000, 3xx - 0, 4xx - 0, 5xx - 0
    others - 0
  Throughput:     1.49GB/s
```

![alt text](string-allocations.png)

## Large string with escaped characters

```sh
.\bombardier.exe -l -n 1000 -c 50 http://localhost:5174/escaped-string
```

```sh
Bombarding http://localhost:5174/escaped-string with 1000 request(s) using 50 connection(s)
 1000 / 1000 [=======================================================================================] 100.00% 253/s 3s
Done!
Statistics        Avg      Stdev        Max
  Reqs/sec       275.26     255.74    1654.64
  Latency      184.30ms    68.00ms   466.25ms
  Latency Distribution
     50%   168.10ms
     75%   207.65ms
     90%   270.62ms
     95%   368.58ms
     99%   429.57ms
  HTTP codes:
    1xx - 0, 2xx - 1000, 3xx - 0, 4xx - 0, 5xx - 0
    others - 0
  Throughput:     1.08GB/s
```

![alt text](escaped-string-char-allocations.png)

![alt text](escaped-string-allocations.png)

