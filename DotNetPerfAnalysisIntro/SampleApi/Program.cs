using SampleApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DataStore>();
var app = builder.Build();

app.MapPost("/readings", async (DataStore store, HttpRequest request) => {
    var data = await DataReader.ReadDataAsync(request.Body);
    store.Update(data);

    return data;
});

app.MapGet("/readings/{location}", (DataStore store, string location) => store.GetStats(location));

app.Run();
