using Entities;
using Services.Helpers;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Countries.Request;
using ServicesContracts.DTO.Countries.Response;
using System.ComponentModel.DataAnnotations;

namespace Services.Countries;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries = [];
    public CountriesService(bool initialize = true)
    {
        if (initialize)
        {
            _countries = new List<Country>
{
    new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Egypt" },
    new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "USA" },
    new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Germany" },
    new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "UK" },
    new() { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "France" },
    new() { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Italy" },
    new() { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Spain" },
    new() { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Canada" },
    new() { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Australia" },
    new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Japan" },
    new() { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "China" },
    new() { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "India" },
    new() { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Brazil" },
    new() { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Mexico" },
    new() { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "UAE" },
    new() { Id = Guid.Parse("12121212-1212-1212-1212-121212121212"), Name = "Saudi Arabia" },
    new() { Id = Guid.Parse("34343434-3434-3434-3434-343434343434"), Name = "South Africa" },
    new() { Id = Guid.Parse("56565656-5656-5656-5656-565656565656"), Name = "Nigeria" },
    new() { Id = Guid.Parse("78787878-7878-7878-7878-787878787878"), Name = "Turkey" },
    new() { Id = Guid.Parse("90909090-9090-9090-9090-909090909090"), Name = "Greece" }
};
        }
    }
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
