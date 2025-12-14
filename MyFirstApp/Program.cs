using MyFirstApp;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseMathUtility();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("#1");
    await next(context);
});

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("#2");
    await next(context);
});

app.Run(async context =>
{
    await context.Response.WriteAsync("Terminating #3");
});

app.Run();
