using MyFirstApp.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.UseMathUtility();

app.UseCustomAuthentication();

app.Run(async context => {
    
});

app.Run();
