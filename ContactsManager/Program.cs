using Services.Countries;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.Persons;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddValidation();
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonsService, PersonsService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
