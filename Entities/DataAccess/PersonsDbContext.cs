using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Entities.DataAccess;
public class PersonsDbContext : DbContext
{
    public PersonsDbContext(DbContextOptions<PersonsDbContext> dbContextOptions) : base(dbContextOptions) { }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");

        modelBuilder.Entity<Person>().ToTable("Persons");

        //foreach (Country country in GetCountriesListFromJson())
        //{
        //    modelBuilder.Entity<Country>().HasData(country);
        //}
        //foreach (Person person in GetPersonsFromJson())
        //{
        //    modelBuilder.Entity<Person>().HasData(person);
        //}
        modelBuilder.Entity<Person>()
            .HasOne(person => person.Country)
            .WithMany(country => country.Persons)
            .HasForeignKey(person => person.CountryId);

        modelBuilder.Entity<Country>()
            .HasIndex(country => country.Name)
            .IsUnique();
    }

    List<Person> GetPersonsFromJson()
    {
        var fileContent = File.ReadAllText("persons.json");
        List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(fileContent);
        return persons ?? [];
    }

    List<Country> GetCountriesListFromJson()
    {
        string fileContent = File.ReadAllText("countries.json");
        List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(fileContent);
        return countries ?? [];
    }

}
