using Services.Countries;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.Persons;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
