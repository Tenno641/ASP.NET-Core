using ServicesContracts.DTO.Countries.Request;
using ServicesContracts.DTO.Countries.Response;

namespace ServicesContracts.Countries;

public interface ICountriesService
{
    CountryResponse AddCountry(CountryRequest countryRequest);
    IEnumerable<CountryResponse> GetAll();
    CountryResponse? Get(Guid? id);
}
