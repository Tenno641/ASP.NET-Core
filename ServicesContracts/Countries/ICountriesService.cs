namespace ServicesContracts.Countries;

public interface ICountriesService
{
    CountryResponse AddCountry(CountryRequest countryRequest);
    IEnumerable<CountryResponse> GetAll();
    CountryResponse? Get(Guid? id);
}
