using Entities;
using Services.Helpers;
using ServicesContracts.Countries;
using System.ComponentModel.DataAnnotations;

namespace Services.Countries;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries = [];
    public CountryResponse AddCountry(CountryRequest? countryRequest)
    {
        ArgumentNullException.ThrowIfNull(countryRequest);

        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(countryRequest);

        if (!objectValidation.isValid)
        {
            string errors = string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage));
            Console.WriteLine(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));
            throw new ArgumentException(errors);
        }

        Country country = countryRequest.ToCountry();
        country.Id = Guid.NewGuid();

        if (isDuplicated(country)) throw new ArgumentException("Country Already Exist.");

        _countries.Add(country);

        return country.ToCountryResponse();
    }   
    public IEnumerable<CountryResponse> GetAll()
    {
        return _countries.Select(country => country.ToCountryResponse());
    }
    public CountryResponse? Get(Guid? id)
    {
        if (id is null) return null;

        Country? country = _countries.FirstOrDefault(country => country.Id == id);
        if (country is null) return null;

        return country.ToCountryResponse();
    }

    private bool isDuplicated(Country country)
    {
        return _countries.Exists(existingCountry => existingCountry.Name == country.Name);
    }
}
