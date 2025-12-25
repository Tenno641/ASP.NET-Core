using Services.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.Persons;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Countries;
using Services.Countries;
using Microsoft.VisualBasic;
namespace ContactsManager.Tests;

public class PersonsServiceTests
{
    private readonly IPersonsService _service;
    private readonly ICountriesService _countriesService;
    public PersonsServiceTests()
    {
        _countriesService = new CountriesService();
        _service = new PersonsService(_countriesService);
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
        PersonRequest request = new() { Name = null };

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
        PersonRequest request = new()
        {
            Name = "User-Name",
            Email = "Example@gmail.com",
            Address = "User-Address",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Parse("2000-02-02"),
            Gender = GenderOptions.Male,
            ReceiveNewsLetter = false
        };

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
        CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
        PersonRequest personRequest = new()
        {
            Name = "User-Name",
            Address = "User-Address",
            CountryId = countryResponse.Id,
            DateOfBirth = DateTime.Parse("2000-02-02"),
            Email = "Example@gmail.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetter = false
        };

        // Act
        PersonResponse personResponse = _service.AddPerson(personRequest);
        PersonResponse? filteredPerson = _service.Get(personResponse.Id);

        // Assert
        Assert.Equal(personResponse, filteredPerson);




    }
    #endregion

}
