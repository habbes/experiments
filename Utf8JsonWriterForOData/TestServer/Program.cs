using ExperimentsLib;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TestServer;

var builder = WebApplication.CreateBuilder(args);

var writers = DefaultServerCollection.Create(null);

// Add services to the container.
builder.Services.AddSingleton<ServerCollection<IEnumerable<Customer>>>(writers);
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new TestOutputFormatter());
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
