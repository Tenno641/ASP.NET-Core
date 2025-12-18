var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/me"),
    app =>
    {
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            using StreamReader reader = new StreamReader(context.Request.Body);
            string rawData = await reader.ReadToEndAsync();
            context.Items["rawData"] = rawData;
            context.Request.Body.Position = 0;
            await next(context);
        });
    });

app.Run();
