using Entities;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository;

public class CountriesRepository : ICountriesRepository
{
    private readonly PersonsDbContext _dbContext;
    public CountriesRepository(PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Country> Add(Country country)
    {
        _dbContext.Countries.Add(country);
        await _dbContext.SaveChangesAsync();
        return country;
    }

    public async Task<IEnumerable<Country>> AddRange(IEnumerable<Country> countries)
    {
        _dbContext.Countries.AddRange(countries);
        await _dbContext.SaveChangesAsync();
        return countries;
    }

    public IQueryable<Country> All()
    {
        return _dbContext.Countries
            .AsNoTracking();
    }

    public async Task<Country?> GetAsync(Guid id)
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Id == id);
    }
}
