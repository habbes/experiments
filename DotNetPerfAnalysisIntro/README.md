# .NET Performance analysis intro

This sample project aims to demonstrate how to analyze and improve the performance of a .NET
service using Visual Studio profilers.

In this exercise we attempt to improve the latency and throughput of a simple API.

To run the load test:

```sh
cd load-tests
k6 run --vus <num_virtual_users> --duration <duration> ./basic.js
```

e.g.

```sh
k6 run --vus 200 --duration 30s ./basic.js
```
