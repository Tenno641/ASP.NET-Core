using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

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
    if (context.Request.Query.ContainsKey("hey"))
    {
        string? query = context.Request.Query["hey"];
        if (query is not null)
        {
            await context.Response.WriteAsync(query);
        }
    }
});

app.Run();
