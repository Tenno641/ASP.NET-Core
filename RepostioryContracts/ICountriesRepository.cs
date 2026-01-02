using Entities;

namespace RepositoryContracts;

public interface ICountriesRepository
{
    Task<Country> Add(Country country);
    Task<IEnumerable<Country>> AddRange(IEnumerable<Country> countries);
    Task<Country?> GetAsync(Guid id);
    IQueryable<Country> All();
}
