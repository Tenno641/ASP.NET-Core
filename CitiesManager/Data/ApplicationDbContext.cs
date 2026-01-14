using CitiesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<City> Cities { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>()
            .Property(city => city.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<City>()
            .HasIndex(city => city.Name);

        SeedCityData(modelBuilder);
    }

    private void SeedCityData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(new City() { Id = Guid.Parse("47C10B7D-1FAB-4CF0-81FC-366AC8E17652"), Name = "Georgia" });

        modelBuilder.Entity<City>()
            .HasData(new City() { Id = Guid.Parse("696D908C-957C-44F4-8098-19BD0EAB4DC1"), Name = "Manchester" });
    }
}
