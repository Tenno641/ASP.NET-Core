using AspRouting.CustomConstraints;
using Microsoft.Extensions.FileProviders;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = "CustomRoot" });
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("isadult", typeof(AdultConstraint));
});
var app = builder.Build();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "assets")) });

app.MapGet("countries", async context =>
{
    ReadOnlyCollection<string> countries = ["United States", "Canada", "United Kingdom", "India", "Japan"];
    await context.Response.WriteAsync(string.Join('\n', countries.Select((country, index) => $"{index + 1}, {country}")));
});

app.MapGet("countries/{id:int}", async context =>
{
    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    ReadOnlyCollection<string> countries = ["United States", "Canada", "United Kingdom", "India", "Japan"];
    if (id <= 0 || id > 100)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
        return;

    }
    else if (id > countries.Count)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("[No Country]");
        return;
    }
    await context.Response.WriteAsync($"{countries[id - 1]}");
});

app.Map("files/{fileName}.{extension}", async context =>
{
    string? fileName = Convert.ToString(context.Request.RouteValues["fileName"]);
    string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
    await context.Response.WriteAsync($"{fileName}.{extension}");
});

app.Map("products/details/{id=1}", async context =>
{
    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    await context.Response.WriteAsync($"Products's id : {id}");
});

app.Map("employees/profile/{employeeName?}", async context =>
{
    string? employeeName = Convert.ToString(context.Request.RouteValues["employeeName"]);
    if (employeeName is null) { await context.Response.WriteAsync("Please provide emp name"); }
    else
    {
        await context.Response.WriteAsync($"{employeeName}'s profile");
    }
});

app.Map("date/{date:datetime?}", async context =>
{
    object? routeValue = context.Request.RouteValues["date"];
    if (routeValue is null) return;
    DateTime dateTime = Convert.ToDateTime(routeValue);
    await context.Response.WriteAsync(dateTime.ToLongDateString() + $" {dateTime.ToLongTimeString()}");
});

app.Map("adults/{date:datetime:isadult}", async context =>
{
    await context.Response.WriteAsync("WOW YOURE'RE ADULT!");
});

app.MapGet("rounds/{**path}", async context =>
{
    foreach (KeyValuePair<string, object?> routes in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"{routes.Key} - {routes.Value}");
    }
});

app.MapFallback(async context =>
{
    await context.Response.WriteAsync("MapFallback");
});
app.Run();
