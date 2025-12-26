using Services.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.Persons;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Countries;
using Services.Countries;

public class PersonsServiceTests
{
    private readonly IPersonsService _service;
    private readonly ICountriesService _countriesService;
    public PersonsServiceTests()
    {
        _countriesService = new CountriesService(false);
        _service = new PersonsService(_countriesService, false);
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
        PersonRequest personRequest = CreatePerson(countryResponse);

        // Act
        PersonResponse personResponse = _service.AddPerson(personRequest);
        PersonResponse? filteredPerson = _service.Get(personResponse.Id);

        // Assert
        Assert.Equal(personResponse, filteredPerson);
    }

    private static PersonRequest CreatePerson(CountryResponse countryResponse)
    {
        return new()
        {
            Name = "Mister-Name",
            Address = "User-Address",
            CountryId = countryResponse.Id,
            DateOfBirth = DateTime.Parse("2000-02-02"),
            Email = "Example@gmail.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetter = false
        };
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

        CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
        CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);

        IEnumerable<PersonRequest> personRequests =
            [
                CreatePerson(countryResponse2),
                CreatePerson(countryResponse1)
            ];

        // Act
        foreach(PersonRequest request in personRequests)
        {
            personResponses.Add(_service.AddPerson(request));
        }
        IEnumerable<PersonResponse> actualPersonResponses = _service.GetAll();

        // Assert
        foreach(PersonResponse person in personResponses)
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

        CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
        CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);

        IEnumerable<PersonRequest> personRequests =
            [
                CreatePerson(countryResponse2),
                CreatePerson(countryResponse1)
            ];

        // Act
        foreach(PersonRequest request in personRequests)
        {
            _service.AddPerson(request);
        }

        IEnumerable<PersonResponse> personResponses = _service.Filter(person => person.Name, name => name?.StartsWith('m') ?? false);
        IEnumerable<PersonResponse> actualPersonResponses = _service.GetAll().Where(person => person.Name?.StartsWith('m') ?? false);

        // Assert
        Assert.Equal(actualPersonResponses, personResponses); 
    }
    #endregion
}
