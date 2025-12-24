using StocksApp.ServiceContracts;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.Add(new ServiceDescriptor(typeof(IFinnhubService), typeof(FinnhubService), ServiceLifetime.Scoped));
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
