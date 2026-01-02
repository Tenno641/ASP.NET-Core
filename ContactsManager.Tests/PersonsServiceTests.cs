using Services.Persons;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.DTO.Countries.Response;
using ServicesContracts.DTO.Countries.Request;
using Microsoft.EntityFrameworkCore;
using Entities.DataAccess;
using Services.Countries;
using Microsoft.Data.Sqlite;
using FluentAssertions;
using AutoFixture;
using ContactsManager.Tests;

public class PersonsServiceTests : IClassFixture<DbContextFixture>
{
    private readonly PersonsService _service;
    private readonly IFixture _fixture;
    private readonly CountriesService _countriesService;
    public PersonsServiceTests()
    {
        _fixture = new Fixture();

        SqliteConnection connection = new SqliteConnection("DataSource=:memory:");
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
        Func<Task> action = () => _service.AddPersonAsync(request);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_ThrowsArgumentException_IfPropertyIsNull()
    {
        // Arrange
        PersonRequest personRequest = new();

        // Act
        Func<Task> action = () => _service.AddPersonAsync(personRequest); 

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddPerson_PersonAdded_IfPersonIsValid()
    {
        // Arrange
        CountryRequest countryRequest = _fixture.Create<CountryRequest>();
        CountryResponse countryResponse = await _countriesService.AddCountryAsync(countryRequest);

        PersonRequest personRequest = _fixture.Build<PersonRequest>()
            .With(person => person.CountryId, countryResponse.Id)
            .With(person => person.Email, "Example@Example.com")
            .Create();

        // Act
        PersonResponse response = await _service.AddPersonAsync(personRequest);
        IEnumerable<PersonResponse> responses = await _service.GetAllAsync();

        // Assert
        response.Id.Should().NotBe(Guid.Empty);
        responses.Should().Contain(response);
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
                await CreatePersonAsync("COLOMBIA"),
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
                await CreatePersonAsync("SWEDEN"),
                await CreatePersonAsync("CHINA")
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
