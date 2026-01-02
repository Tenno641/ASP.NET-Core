using Services.Countries;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.Persons;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using Rotativaio.AspNetCore;
using RepositoryContracts;
using Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddValidation();

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddRotativaIo("https://api.rotativa.io", builder.Configuration["rotativaApiKey"] ?? throw new InvalidOperationException("RotativaApiKey is missing"));
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
