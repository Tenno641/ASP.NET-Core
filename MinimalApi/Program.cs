using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using MinimalApi.DTO.Products;
using MinimalApi.Models;
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

app.MapGet("/{value:alpha?}", async (HttpContext context, [FromQuery] string? value) =>
{
    await context.Response.WriteAsync(value ?? "Empty Value!");
});

app.MapPost("products", async (HttpContext context, AddProductRequest productRequest, ApplicationDbContext dbContext) =>
{
    Product product = new Product()
    {
        Name = productRequest.Name
    };
    dbContext.Products.Add(product);
    await dbContext.SaveChangesAsync();
    await context.Response.WriteAsJsonAsync(product);
});

app.MapGet("products{id:guid}", async (HttpContext context, Guid id, ApplicationDbContext dbContext) =>
{
    Product? product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    if (product is null)
    {
        await context.Response.WriteAsync("Product Not Found");
        return;
    }

    await context.Response.WriteAsJsonAsync(product);
});

app.MapGet("products", async (HttpContext context, ApplicationDbContext dbContext) =>
{
    IEnumerable<Product> products = await dbContext.Products.AsNoTracking().ToListAsync();

    await context.Response.WriteAsJsonAsync(products);
});

app.Run();
