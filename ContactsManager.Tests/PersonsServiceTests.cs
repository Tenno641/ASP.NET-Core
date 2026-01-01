using Services.Persons;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.DTO.Countries.Response;
using ServicesContracts.DTO.Countries.Request;
using Moq;
using Microsoft.EntityFrameworkCore;
using Entities.DataAccess;
using Services.Countries;
using Microsoft.Data.Sqlite;

public class PersonsServiceTests
{
    private readonly PersonsService _service;
    private readonly CountriesService _countriesService;
    public PersonsServiceTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PersonsDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new PersonsDbContext(options);
        context.Database.EnsureCreated();

        _service = new PersonsService(context);
        _countriesService = new CountriesService(context);
    }

    #region Add Person
    [Fact]
    public async Task AddPerson_ThrowsArgumentNull_ArgumentIsNull()
    {
        PersonRequest? request = null;
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddPersonAsync(request));
    }

    [Fact]
    public async Task AddPerson_ThrowsArgumentException_IfPropertyIsNull()
    {
        // Arrange
        CountryRequest countryRequest = new CountryRequest() { Name = "London" };
        CountryResponse countryResponse = await _countriesService.AddCountryAsync(countryRequest);
        PersonRequest request = new();

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _service.AddPersonAsync(request);
        });
    }

    [Fact]
    public async Task AddPerson_PersonAdded_IfPersonIsValid()
    {
        // Arrange
        PersonRequest request = await CreatePersonAsync("London");
        
        // Act
        PersonResponse response = await _service.AddPersonAsync(request);
        IEnumerable<PersonResponse> responses = await _service.GetAllAsync();

        // Assert
        Assert.True(response.Id != Guid.Empty);
        Assert.Contains(response, responses);
    }
    #endregion

    #region Get Person
    [Fact]
    public async Task Get_ReturnsNull_IfIdIsNull()
    {
        Guid? id = null;
        PersonResponse? response = await _service.GetAsync(id);
        Assert.Null(response);
    }

    [Fact]
    public async Task Get_ValidPersonResponseObject_ProvidingValidId()
    {
        // Arrange
        PersonRequest personRequest = await CreatePersonAsync("London");

        // Act
        PersonResponse personResponse = await _service.AddPersonAsync(personRequest);
        PersonResponse? filteredPerson = await _service.GetAsync(personResponse.Id);

        // Assert
        Assert.Equal(personResponse, filteredPerson);
    }

    private async Task<PersonRequest> CreatePersonAsync(string countryName)
    {
        CountryRequest countryRequest = new() { Name = countryName };
        CountryResponse countryResponse = await _countriesService.AddCountryAsync(countryRequest);

        return new()
        {
            Name = "User-Name",
            Email = "Example@gmail.com",
            Address = "User-Address",
            CountryId = countryResponse.Id,
            DateOfBirth = DateTime.Parse("2000-01-02"),
            Gender = GenderOptions.Male,
            ReceiveNewsLetter = false
        };
    }
    #endregion

    #region GetAll
    [Fact]
    public async Task GetAll_ReturnsEmptyList_NoAddedPersons()
    {
        IEnumerable<PersonResponse> personResponses = await _service.GetAllAsync();
        Assert.Empty(personResponses);
    }

    [Fact]
    public async Task GetAll_ReturnPersons_IfWeAddedValidPersons()
    {
        // Arrange
        List<PersonResponse> personResponses = [];

        IEnumerable<PersonRequest> personRequests =
            [
                await CreatePersonAsync("London"),
                await CreatePersonAsync("USA")
            ];

        // Act
        foreach (PersonRequest request in personRequests)
        {
            personResponses.Add(await _service.AddPersonAsync(request));
        }
        IEnumerable<PersonResponse> actualPersonResponses = await _service.GetAllAsync();

        // Assert
        foreach (PersonResponse person in personResponses)
        {
            Assert.Contains(person, actualPersonResponses);
        }
    }
    #endregion

    #region Filter
    [Fact]
    public async Task Filter_GetFilteredPersons()
    {
        // Arrange
        IEnumerable<PersonRequest> personRequests =
            [
                await CreatePersonAsync("London"),
                await CreatePersonAsync("USA")
            ];

        // Act
        foreach (PersonRequest request in personRequests)
        {
            await _service.AddPersonAsync(request);
        }

        IEnumerable<PersonResponse> personResponses = await _service.FilterAsync("Name", "m");
        IEnumerable<PersonResponse> actualPersonResponses = (await _service.GetAllAsync()).Where(person => person.Name?.StartsWith('m') ?? false);

        // Assert
        Assert.Equal(actualPersonResponses, actualPersonResponses);
    }
    #endregion
}
