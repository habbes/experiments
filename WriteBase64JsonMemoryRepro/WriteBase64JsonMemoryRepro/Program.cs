// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;


var stopwatch = Stopwatch.StartNew();
await Run();
stopwatch.Stop();
Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds}ms");

static Task Run(int numTasks = 20_000)
{
    byte[] data = GenerateData(1024 * 1024);

    Task[] tasks = new Task[numTasks];
    for (int i = 0; i < numTasks; i++)
    {
        tasks[i] = Task.Run(() => Write(data));
    }

    return Task.WhenAll(tasks);
}



static void Write(byte[] data)
{
    MemoryStream stream = new MemoryStream();
    Utf8JsonWriter writer = new(stream);
    writer.WriteBase64StringValue(data.AsSpan());
}


[MethodImpl(MethodImplOptions.NoInlining)]
static byte[] GenerateData(int length)
{
    byte[] data = new byte[length];
    for (int i = 0; i < length; i++)
    {
        data[i] = (byte)(i % 256);
    }

    return data;
}