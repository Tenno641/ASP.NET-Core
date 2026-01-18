using Microsoft.EntityFrameworkCore;
using MinimalApi.Data;
using MinimalApi.DTO.Products;
using MinimalApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Products;

public static class Endpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("products", async (HttpContext context, AddProductRequest productRequest, ApplicationDbContext dbContext) =>
        {
            Product product = new Product()
            {
                Name = productRequest.Name
            };
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            await context.Response.WriteAsJsonAsync(product);
        }).AddEndpointFilter<ValidationEndpointFilter>();

        group.MapGet("products{id:guid}", async (HttpContext context, Guid id, ApplicationDbContext dbContext) =>
        {
            Product? product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                await context.Response.WriteAsync("Product Not Found");
                return;
            }

            await context.Response.WriteAsJsonAsync(product);
        });

        group.MapGet("products", async (HttpContext context, ApplicationDbContext dbContext) =>
        {
            IEnumerable<Product> products = await dbContext.Products.AsNoTracking().ToListAsync();

            await context.Response.WriteAsJsonAsync(products);
        });

        return group;
    }
}
