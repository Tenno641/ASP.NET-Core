using AutoFixture;
using Azure;
using ContactsManager.Tests;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Services.Countries;
using Services.Persons;
using ServicesContracts.DTO.Persons.Response;
using System.Linq.Expressions;
using Xunit.Abstractions;

public class PersonsServiceTests : IClassFixture<DbContextFixture>
{
    private readonly PersonsService _personsService;
    private readonly CountriesService _countriesService;
    private readonly Fixture _fixture;
    private readonly Mock<IPersonsRepository> _personsRepositoryMock;
    private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
    private readonly ITestOutputHelper _testOutput;
    public PersonsServiceTests(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
        _fixture = new Fixture();

        _personsRepositoryMock = new Mock<IPersonsRepository>();
        _countriesRepositoryMock = new Mock<ICountriesRepository>();

        _personsService = new PersonsService(_personsRepositoryMock.Object);
        _countriesService = new CountriesService(_countriesRepositoryMock.Object);
    }

    #region Add Person
    [Fact]
    public async Task AddPerson_ThrowsArgumentNull_ArgumentIsNull()
    {
        PersonRequest? person = null;

        // Act
        Func<Task> action = () => _personsService.AddPersonAsync(person!);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_ThrowsArgumentException_IfPropertyIsNull()
    {
        // Arrange
        PersonRequest personRequest = new();

        // Act
        Func<Task> action = () => _personsService.AddPersonAsync(personRequest);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddPerson_PersonAdded_IfPersonIsValid()
    {
        // Arrange 
        PersonRequest personRequest = _fixture.Build<PersonRequest>()
            .With(person => person.Email, "Example@Example.con")
            .Create();

        Person person = personRequest.ToPerson();
        PersonResponse expected = PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);

        _personsRepositoryMock
            .Setup(repo => repo.AddPersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        // Act
        PersonResponse response = await _personsService.AddPersonAsync(personRequest);
        expected.Id = response.Id;

        // Assert
        response.Id.Should().NotBe(Guid.Empty);
        expected.Should().Be(response);
    }
    #endregion

    #region Get Person
    [Fact]
    public async Task Get_ReturnsNull_IfIdIsNull()
    {
        Guid? id = null;
        PersonResponse? response = await _personsService.GetAsync(id);
        Assert.Null(response);
    }

    [Fact]
    public async Task Get_ValidPersonResponseObject_ProvidingValidId()
    {
        // Arrange
        PersonRequest personRequest = _fixture.Build<PersonRequest>().With(person => person.Email, "example@example.com").Create();
        Person person = personRequest.ToPerson();

        _personsRepositoryMock
            .Setup(repo => repo.AddPersonAsync(It.IsAny<Person>()))
            .ReturnsAsync(person);

        _personsRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        // Act
        PersonResponse personResponse = await _personsService.AddPersonAsync(personRequest);
        PersonResponse? filteredPerson = await _personsService.GetAsync(personResponse.Id);
        PersonResponse expected = filteredPerson.Value with { Id = personResponse.Id };

        // Assert
        expected.Should().Be(personResponse);
    }
    #endregion

    #region GetAll
    [Fact]
    public async Task GetAll_ReturnsEmptyList_NoAddedPersons()
    {
        // Arrange
        _personsRepositoryMock
            .Setup(repo => repo.AllAsync())
            .ReturnsAsync([]);

        // Assert
        (await _personsService.GetAllAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ReturnPersons_IfWeAddedValidPersons()
    {
        // Arrange
        IEnumerable<PersonRequest> personRequests = _fixture.CreateMany<PersonRequest>();
        IEnumerable<Person> persons = personRequests.Select(person => person.ToPerson());

        _personsRepositoryMock
            .Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<Person>>()))
            .ReturnsAsync(persons);

        _personsRepositoryMock
            .Setup(repo => repo.AllAsync())
            .ReturnsAsync(persons);

        // Act
        IEnumerable<PersonResponse> personResponses = await _personsService.AddRangeAsync(personRequests);
        IEnumerable<PersonResponse> actualPersonResponses = await _personsService.GetAllAsync();

        // Assert
        actualPersonResponses.Should().BeEquivalentTo(personResponses);
    }

    #endregion

    #region Filter
    [Fact]
    public async Task Filter_GetFilteredPersons()
    {
        // Arrange
        IEnumerable<PersonRequest> personRequests =
            [
                _fixture.Build<PersonRequest>().With(person => person.Name, "Mamdani").Create(),
                _fixture.Build<PersonRequest>().With(person => person.Name, "Rufus").Create(),
                _fixture.Build<PersonRequest>().With(person => person.Name, "motor").Create(),
            ];
        IEnumerable<Person> persons = personRequests.Where(person => person.Name == null || person.Name.Contains('m')).Select(person => person.ToPerson());

        _personsRepositoryMock
            .Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(persons);

        _personsRepositoryMock
            .Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<Person>>()))
            .ReturnsAsync(persons);

        _personsRepositoryMock
            .Setup(repo => repo.AllAsync())
            .ReturnsAsync(persons);

        IEnumerable<PersonResponse> responses = await _personsService.AddRangeAsync(personRequests);

        // Act
        IEnumerable<PersonResponse> expectedResponses = await _personsService.FilterAsync("Name", "m");
        IEnumerable<PersonResponse> actualResponses = persons.Select(person => PersonResponseExtension.ToPersonResponse.Compile().Invoke(person));

        _testOutput.WriteLine(string.Join("\n", expectedResponses));

        // Assert
        actualResponses.Should().BeEquivalentTo(expectedResponses);
    }
    #endregion
}
