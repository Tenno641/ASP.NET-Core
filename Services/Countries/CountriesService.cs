using Entities;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Countries.Request;
using ServicesContracts.DTO.Countries.Response;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Services.Countries;

public class CountriesService : ICountriesService
{
    private readonly PersonsDbContext _dbContext;
    public CountriesService(PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CountryResponse> AddCountryAsync(CountryRequest? countryRequest)
    {
        ArgumentNullException.ThrowIfNull(countryRequest);

        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(countryRequest);

        if (!objectValidation.isValid)
        {
            string errors = string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage));
            throw new ArgumentException(errors);
        }

        Country country = countryRequest.ToCountry();
        country.Id = Guid.NewGuid();

        if (await isDuplicated(country)) throw new ArgumentException("Country Already Exist.");

        _dbContext.Countries.Add(country);
        await _dbContext.SaveChangesAsync();

        return country.ToCountryResponse();
    }
    public async Task<IEnumerable<CountryResponse>> GetAllAsync()
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .Select(ToResponseExpression)
            .ToListAsync();
    }
    public async Task<CountryResponse?> GetAsync(Guid? id)
    {
        if (id is null) return null;

        return await _dbContext.Countries
           .AsNoTracking()
           .Where(country => country.Id == id)
           .Select(ToResponseExpression)
           .FirstOrDefaultAsync();
    }
    private static readonly Expression<Func<Country, CountryResponse>> ToResponseExpression =
    country => new CountryResponse { Id = country.Id, Name = country.Name };
    private async Task<bool> isDuplicated(Country country)
    {
        string? trimmedName = country.Name?.Trim();
        return await _dbContext.Countries.AnyAsync(existingCountry => existingCountry.Name == trimmedName);
    }
}
