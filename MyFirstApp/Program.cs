using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/cached", async context =>
{
    string message = "Hello in bytes";
    byte[] buffer = Encoding.UTF8.GetBytes(message);
    await context.Response.Body.WriteAsync(buffer);
});

app.Run(async context =>
{
    using StreamReader reader = new StreamReader(context.Request.Body);
    string query = await reader.ReadToEndAsync();
    Dictionary<string, StringValues> stringQueries = QueryHelpers.ParseQuery(query);
    int counter = 1;
    foreach (KeyValuePair<string, StringValues> keyValue in stringQueries)
    {
        await context.Response.WriteAsync($"query #{counter++} : {keyValue.Key} - {keyValue.Value}");
    }
});

app.Run();
