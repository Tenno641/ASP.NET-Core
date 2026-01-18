using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using MinimalApi.Products;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres"));
});
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGroup("/products").MapProductEndpoints();

app.MapPost("/is-palindrome{input:alpha}", (string input) =>
{
    int length = input.Length;
    for (int i = 0; i < length / 2; i++)
    {
        bool shallMatch = input[i].Equals(input[length - i - 1]);
        if (!shallMatch) return Results.Ok(false);
    }

    return Results.Ok(true);

    /*StringBuilder reversed = new();
    for (int i = input.Length - 1; i >= 0; i--)
    {
        reversed.Append(input[i]);
    }
    bool isPalindrome = input.Equals(reversed.ToString(), StringComparison.InvariantCultureIgnoreCase);
    return Results.Ok(isPalindrome);
    */
});

app.Run();
