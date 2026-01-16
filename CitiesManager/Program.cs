using Asp.Versioning;
using CitiesManager.Data;
using CitiesManager.Models.IdentityEntities;
using CitiesManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters();

builder.Services.AddOpenApi("v1");
builder.Services.AddOpenApi("v2");
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes.Add("bearer", new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        return Task.CompletedTask;
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configure =>
    {
        configure.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSqlServer"));
});

builder.Services.AddTransient<JwtService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine(context.Exception.Message);
            Console.WriteLine(context.Exception.ToString());
            return Task.CompletedTask;
        },
    };

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Please provide token secret key")))
    };

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddDocument("v1", "Version 1.0", "openapi/v1.json");
        options.AddDocument("v2", "Version 2.0", "openapi/v2.json");
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
