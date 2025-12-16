using AspRouting.CustomConstraints;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = "CustomRoot" });
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("isadult", typeof(AdultConstraint));
});
var app = builder.Build();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "assets"))});

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
