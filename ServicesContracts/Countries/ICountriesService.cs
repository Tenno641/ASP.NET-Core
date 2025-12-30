using ServicesContracts.DTO.Countries.Request;
using ServicesContracts.DTO.Countries.Response;

namespace ServicesContracts.Countries;

public interface ICountriesService
{
    Task<CountryResponse> AddCountryAsync(CountryRequest countryRequest);
    Task<IEnumerable<CountryResponse>> GetAllAsync();
    Task<CountryResponse?> GetAsync(Guid? id);
}
