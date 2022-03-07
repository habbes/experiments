# Investigate Utf8JsonWriter for OData

This is a set of experiments to support my invesigations of whether the OData writer and serializers can make use of [Utf8JsonWriter](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonwriter) in order to boost performance, and if so, what design changes would it take to make that possible, and what impact would it have on flexibility, extensibility, maintainability, etc.

- [Getting familiar with Utf8JsonWriter](./Utf8JsonWriterSamples/)