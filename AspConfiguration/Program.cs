using AspConfiguration.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");
IEnumerable<KeyValuePair<string, string?>>? inMemoryConfig =
    [
        new KeyValuePair<string, string?>("Person:Name", "User-Name-In-Memory")
    ];
//builder.Configuration.AddInMemoryCollection(inMemoryConfig);
builder.Services.AddControllersWithViews();
builder.Services.Configure<Person>(builder.Configuration.GetSection("Person"));
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
