using Services.Countries;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.Persons;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using Rotativaio.AspNetCore;
using RepositoryContracts;
using Repository;
using Serilog;
using ContactsManager.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders().AddConsole();
builder.Host.UseSerilog((HostBuilderContext hostBuilder, IServiceProvider services, LoggerConfiguration configureLogger) =>
{
    configureLogger.ReadFrom.Configuration(hostBuilder.Configuration);
    configureLogger.ReadFrom.Services(services);
});

builder.Services.AddControllersWithViews(options =>
{
    ILogger<CustomActionFilters> logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<CustomActionFilters>>();

    options.Filters.Add(new CustomActionFilters(logger));
});

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.None;
});

if (!builder.Environment.IsEnvironment("IntegrationTesting"))
{
    builder.Services.AddRotativaIo("https://api.rotativa.io", builder.Configuration["rotativaApiKey"] ?? throw new InvalidOperationException("RotativaApiKey is missing"));
}

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.MapControllers();

app.Run();
