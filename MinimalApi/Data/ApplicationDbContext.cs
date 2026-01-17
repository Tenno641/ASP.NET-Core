using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .ValueGeneratedNever();
    }
}
