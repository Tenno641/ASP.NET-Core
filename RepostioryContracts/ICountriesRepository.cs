using Entities;

namespace RepositoryContracts;

public interface ICountriesRepository
{
    Task<Country> AddAsync(Country country);
    Task<IEnumerable<Country>> AddRangeAsync(IEnumerable<Country> countries);
    Task<Country?> GetAsync(Guid id);
    Task<IEnumerable<Country>> AllAsync();
}
