using ExperimentsLib;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace TestServer;

public class TestOutputFormatter : TextOutputFormatter
{
    public TestOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));

        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type? type)
        => typeof(IEnumerable<Customer>).IsAssignableFrom(type);

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var httpContext = context.HttpContext;
        var services = httpContext.RequestServices;

        var logger = services.GetRequiredService<ILogger<TestOutputFormatter>>();
        var writers = services.GetRequiredService<ServerCollection<IEnumerable<Customer>>>();

        // the writer is specified in the route: customers/{writer}
        var writerName = httpContext.Request.RouteValues["writer"] as string;
        logger.LogInformation($"Looking for requested writer '{writerName}'");
        var writer = writers.GetWriter(writerName);

        logger.LogInformation($"Using writer '{writerName}' for serialization");
        var customers = (IEnumerable<Customer>)context.Object!;

        var stream = httpContext.Response.Body;

        await writer.WritePayload(customers, stream);
    }
}

