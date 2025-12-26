using Services.Countries;
using ServicesContracts.Countries;

namespace ContactsManager.Tests;

public class CountriesServiceTests
{
    private readonly ICountriesService _service;
    public CountriesServiceTests()
    {
        _service = new CountriesService(false);
    }

    #region AddCountryTests
    [Fact]
    public void AddCountry_ReturnsNull_IfArgumentIsNull()
    {
        // Arrange
        CountryRequest? request = null;

        // Act
        Assert.Throws<ArgumentNullException>(() => _service.AddCountry(request));
    }

    [Fact]
    public void AddCountry_ThrowsArgumentNull_IfPropertyIsNull()
    {
        CountryRequest request = new CountryRequest() { Name = null };
        Assert.Throws<ArgumentException>(() => _service.AddCountry(request));
    }

    [Fact]
    public void AddCountry_ThrowsArgument_IfCountryAlreadyExists()
    {
        CountryRequest request = new CountryRequest() { Name = "USA" };
        CountryRequest request2 = new CountryRequest() { Name = "USA" };
        Assert.Throws<ArgumentException>(() =>
        {
            _service.AddCountry(request);
            _service.AddCountry(request2);
        });
    }

    [Fact]
    public void AddCountry_AddCountryToRepository()
    {
        // Arrange 
        CountryRequest request = new CountryRequest() { Name = "GHANA" };

        // Act
        CountryResponse response = _service.AddCountry(request);
        IEnumerable<CountryResponse> actualCountries = _service.GetAll();

        // Assert
        Assert.Contains(response, actualCountries);
        Assert.True(response.Id != Guid.Empty);
    }
    #endregion

    #region GetAll
    [Fact]
    public void GetAll_ReturnsEmptyList_NotAddingAnyData()
    {
        IEnumerable<CountryResponse> actualCountries = _service.GetAll();
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
            resultCountries.Add(_service.AddCountry(countryRequest)); // COLOMBIA BEING ADDED TWICE!
        }

        IEnumerable<CountryResponse> actualCountries = _service.GetAll();

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
        CountryResponse? countryResponse = _service.Get(guid);
        Assert.Null(countryResponse);
    }

    [Fact]
    public void Get_ReturnCountry_IfIdIsValid()
    {
        // Arrange
        CountryRequest request = new CountryRequest() { Name = "LONDON" };
        CountryResponse response = _service.AddCountry(request);

        // Act
        CountryResponse? countryResponse = _service.Get(response.Id);

        // Assert
        Assert.Equal(countryResponse, response);
    }
    #endregion
}
