using System.Text;

var builder = WebApplication.CreateBuilder(args);

// generate sample data once to reduce noise
// from profiler's allocations data
StringPayload simpleStringPayload = GenerateStringPayload(1_000_000);
StringPayload specialStringPayload = GenerateSpecialStringPayload(1_000_000);
BinaryPayload binaryPayload = GenerateBinaryPayload(1_000_000);

var app = builder.Build();

app.MapGet("/binary", () => binaryPayload);
app.MapGet("/string", () => simpleStringPayload);
app.MapGet("/escaped-string", () => specialStringPayload);

app.Run();

StringPayload GenerateStringPayload(int length)
{
    string data = new string('a', length);
    return new StringPayload(data);
}

StringPayload GenerateSpecialStringPayload(int minLength)
{
    string baseString = "/🌄";
    StringBuilder data = new();
    while (data.Length < minLength)
    {
    data.Append(baseString);
    }
    return new StringPayload(data.ToString());
}

BinaryPayload GenerateBinaryPayload(int length)
{
    byte[] data = new byte[length];
    for (int i = 0; i < length; i++)
    {
    data[i] = (byte)i;
    }

    return new BinaryPayload(data);
}

internal record StringPayload(string Data);

internal record BinaryPayload(byte[] Data);

