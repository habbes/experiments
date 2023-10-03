using JsonWriterPerf;

namespace JsonWriterTests;

public class JsonWriterTests
{

    private static string expectedJson = File.ReadAllText("ExpectedJson.txt");
    [Fact]
    public void JsonNodeSerializationIsCorrect()
    {
        var bench = new Benchmarks();
        bench.Setup();
        bench.SerializePayloadJsonNode();
        var contents = bench.ReadStream();
        Assert.Equal(expectedJson, contents);
    }

    [Fact]
    public void JObjectSerializationIsCorrect()
    {
        var bench = new Benchmarks();
        bench.Setup();
        bench.SerializePayloadJsonNet();
        var contents = bench.ReadStream();
        Assert.Equal(expectedJson, contents);
    }

    [Fact]
    public void JsonElementSerializationIsCorrect()
    {
        var bench = new Benchmarks();
        bench.Setup();
        bench.SerializeJsonElement();
        var contents = bench.ReadStream();
        Assert.Equal(expectedJson, contents);
    }

    [Fact]
    public void ExtensibleJsonSerializationIsCorrect()
    {
        var bench = new Benchmarks();
        bench.Setup();
        bench.SerializeExtensibleJsonElement();
        var contents = bench.ReadStream();
        Assert.Equal(expectedJson, contents);
    }
}