using Services.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.Persons;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Countries;
using Services.Countries;
using ServicesContracts.DTO.Countries.Response;
using ServicesContracts.DTO.Countries.Request;
using Entities.DataAccess;
using Microsoft.EntityFrameworkCore;

public class PersonsServiceTests
{
    private readonly IPersonsService _service;
    private readonly ICountriesService _countriesService;
    public PersonsServiceTests()
    {
        PersonsDbContext personsDbContext = new PersonsDbContext(new DbContextOptions<PersonsDbContext>());
        _countriesService = new CountriesService(personsDbContext);
        _service = new PersonsService(_countriesService, personsDbContext);
    }

    #region Add Person
    [Fact]
    public void AddPerson_ThrowsArgumentNull_ArgumentIsNull()
    {
        PersonRequest? request = null;
        Assert.Throws<ArgumentNullException>(() => _service.AddPerson(request));
    }

    [Fact]
    public void AddPerson_ThrowsArgumentException_IfPropertyIsNull()
    {
        // Arrange
        PersonRequest request = new(null, null, null, null, null, null, false);

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _service.AddPerson(request);
        });
    }

    [Fact]
    public void AddPerson_AddPerson_()
    {
        // Arrange
        PersonRequest request = new
       (
            "User-Name",
            "Example@gmail.com",
            DateTime.Parse("2000-02-02"),
            GenderOptions.Male,
            Guid.NewGuid(),
            "User-Address",
            false
        );

        // Act
        PersonResponse response = _service.AddPerson(request);
        IEnumerable<PersonResponse> responses = _service.GetAll();

        // Assert
        Assert.True(response.Id != Guid.Empty);
        Assert.Contains(response, responses);
    }
    #endregion

    #region Get Person
    [Fact]
    public void Get_ReturnsNull_IfIdIsNull()
    {
        Guid? id = null;
        PersonResponse? response = _service.Get(id);
        Assert.Null(response);
    }

    [Fact]
    public void Get_ValidPersonResponseObject_ProvidingValidId()
    {
        // Arrange
        CountryRequest countryRequest = new() { Name = "London" };
        CountryResponse countryResponse = _countriesService.AddCountryAsync(countryRequest);
        PersonRequest personRequest = CreatePerson(countryResponse);

        // Act
        PersonResponse personResponse = _service.AddPerson(personRequest);
        PersonResponse? filteredPerson = _service.Get(personResponse.Id);

        // Assert
        Assert.Equal(personResponse, filteredPerson);
    }

    private static PersonRequest CreatePerson(CountryResponse countryResponse)
    {
        return new
       (
            "User-Name",
            "Example@gmail.com",
            DateTime.Parse("2000-02-02"),
            GenderOptions.Male,
            Guid.NewGuid(),
            "User-Address",
            false
        );
    }
    #endregion

    #region GetAll
    [Fact]
    public void GetAll_ReturnsEmptyList_NoAddedPersons()
    {
        IEnumerable<PersonResponse> personResponses = _service.GetAll();
        Assert.Empty(personResponses);
    }

    [Fact]
    public void GetAll_ReturnPersons_IfWeAddedValidPersons()
    {
        // Arrange
        List<PersonResponse> personResponses = [];

        CountryRequest countryRequest1 = new() { Name = "London" };
        CountryRequest countryRequest2 = new() { Name = "USA" };

        CountryResponse countryResponse1 = _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse countryResponse2 = _countriesService.AddCountryAsync(countryRequest2);

        IEnumerable<PersonRequest> personRequests =
            [
                CreatePerson(countryResponse2),
                CreatePerson(countryResponse1)
            ];

        // Act
        foreach (PersonRequest request in personRequests)
        {
            personResponses.Add(_service.AddPerson(request));
        }
        IEnumerable<PersonResponse> actualPersonResponses = _service.GetAll();

        // Assert
        foreach (PersonResponse person in personResponses)
        {
            Assert.Contains(person, actualPersonResponses);
        }
    }
    #endregion

    #region Filter
    [Fact]
    public void Filter_GetFilteredPersons()
    {
        // Arrange

        CountryRequest countryRequest1 = new() { Name = "London" };
        CountryRequest countryRequest2 = new() { Name = "USA" };

        CountryResponse countryResponse1 = _countriesService.AddCountryAsync(countryRequest1);
        CountryResponse countryResponse2 = _countriesService.AddCountryAsync(countryRequest2);

        IEnumerable<PersonRequest> personRequests =
            [
                CreatePerson(countryResponse2),
                CreatePerson(countryResponse1)
            ];

        // Act
        foreach (PersonRequest request in personRequests)
        {
            _service.AddPerson(request);
        }

        IEnumerable<PersonResponse> personResponses = _service.Filter(_service.GetAll(), "Name", "m");
        IEnumerable<PersonResponse> actualPersonResponses = _service.GetAll().Where(person => person.Name?.StartsWith('m') ?? false);

        // Assert
        Assert.Equal(actualPersonResponses, personResponses);
    }
    #endregion
}
