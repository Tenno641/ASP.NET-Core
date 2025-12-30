using Services.Countries;
using ServicesContracts.Countries;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;
using ServicesContracts.DTO.Countries.Request;
using ServicesContracts.DTO.Countries.Response;

namespace ContactsManager.Tests;

public class CountriesServiceTests
{
    private readonly ICountriesService _service;
    public CountriesServiceTests()
    {
        _service = new CountriesService(new PersonsDbContext(new DbContextOptions<PersonsDbContext>()));
    }

    #region AddCountryTests
    [Fact]
    public void AddCountry_ReturnsNull_IfArgumentIsNull()
    {
        // Arrange
        CountryRequest? request = null;

        // Act
        Assert.Throws<ArgumentNullException>(() => _service.AddCountryAsync(request));
    }

    [Fact]
    public void AddCountry_ThrowsArgumentNull_IfPropertyIsNull()
    {
        CountryRequest request = new CountryRequest() { Name = null };
        Assert.Throws<ArgumentException>(() => _service.AddCountryAsync(request));
    }

    [Fact]
    public void AddCountry_ThrowsArgument_IfCountryAlreadyExists()
    {
        CountryRequest request = new CountryRequest() { Name = "USA" };
        CountryRequest request2 = new CountryRequest() { Name = "USA" };
        Assert.Throws<ArgumentException>(() =>
        {
            _service.AddCountryAsync(request);
            _service.AddCountryAsync(request2);
        });
    }

    [Fact]
    public void AddCountry_AddCountryToRepository()
    {
        // Arrange 
        CountryRequest request = new CountryRequest() { Name = "GHANA" };

        // Act
        CountryResponse response = _service.AddCountryAsync(request);
        IEnumerable<CountryResponse> actualCountries = _service.GetAllAsync();

        // Assert
        Assert.Contains(response, actualCountries);
        Assert.True(response.Id != Guid.Empty);
    }
    #endregion

    #region GetAll
    [Fact]
    public void GetAll_ReturnsEmptyList_NotAddingAnyData()
    {
        IEnumerable<CountryResponse> actualCountries = _service.GetAllAsync();
        Assert.Empty(actualCountries);
    }

    [Fact]
    public void GetAll_ReturnExistingCountries_AddingCountries()
    {
        List<CountryResponse> resultCountries = [];
        IEnumerable<CountryRequest> countriesRequests =
            [
                new CountryRequest(){Name = "COLOMBIA"},
                new CountryRequest(){Name = "LONDON"},
                new CountryRequest(){Name = "GERMANY"}
            ];

        foreach (CountryRequest countryRequest in countriesRequests)
        {
            resultCountries.Add(_service.AddCountryAsync(countryRequest)); // COLOMBIA BEING ADDED TWICE!
        }

        IEnumerable<CountryResponse> actualCountries = _service.GetAllAsync();

        foreach (CountryResponse expectedCountry in resultCountries)
        {
            Assert.Contains(expectedCountry, actualCountries);
        }
    }

    #endregion

    #region Get
    [Fact]
    public void Get_ReturnNull_IfIdIsNull()  
    {
        Guid? guid = null;
        CountryResponse? countryResponse = _service.GetAsync(guid);
        Assert.Null(countryResponse);
    }

    [Fact]
    public void Get_ReturnCountry_IfIdIsValid()
    {
        // Arrange
        CountryRequest request = new CountryRequest() { Name = "LONDON" };
        CountryResponse response = _service.AddCountryAsync(request);

        // Act
        CountryResponse? countryResponse = _service.GetAsync(response.Id);

        // Assert
        Assert.Equal(countryResponse, response);
    }
    #endregion
}
