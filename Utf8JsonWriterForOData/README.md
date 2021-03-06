# Investigate Utf8JsonWriter for OData

This is a set of experiments to support my invesigations of whether the OData writer and serializers can make use of [Utf8JsonWriter](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonwriter) in order to boost performance, and if so, what design changes would it take to make that possible, and what impact would it have on flexibility, extensibility, maintainability, etc.

This is a follow-up to the findings obtained from the [ODataWriter vs System.Text.Json Performance](../ODataWriterVsSystemTextJson/) experiments.

- [Getting familiar with Utf8JsonWriter](./writers-overview.md)
- [Benchmark Results](./benchmarks-results.md)
- [Load test results](./loadtests-results.md)
- [A list of proposed experiments to explore](./experiment-proposals.md)