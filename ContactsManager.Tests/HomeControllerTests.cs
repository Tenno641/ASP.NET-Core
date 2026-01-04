using AutoFixture;
using ContactsManager.Controllers;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Countries;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using Xunit.Abstractions;

namespace ContactsManager.Tests;

public class HomeControllerTests
{
    private readonly ITestOutputHelper _testOutput;
    private readonly Fixture _fixture;
    private readonly HomeController _homeController;
    private readonly Mock<IPersonsService> _personsServiceMock;
    public HomeControllerTests(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
        _fixture = new Fixture();

        _personsServiceMock = new Mock<IPersonsService>();
        Mock<ICountriesService> countriesServiceMock = new Mock<ICountriesService>();

        _homeController = new HomeController(_personsServiceMock.Object, countriesServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_OkResponse()
    {
        // Arrange
        IEnumerable<PersonResponse> persons = _fixture
            .Build<PersonResponse>()
            .CreateMany();

        _personsServiceMock
            .Setup(service => service.FilterAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(persons);

        _personsServiceMock
            .Setup(service => service.OrderAsync(It.IsAny<IEnumerable<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
            .ReturnsAsync(persons);
        
        // Act
        IActionResult actionResult = await _homeController.Index(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>());

        // Assert
        actionResult.Should().BeOfType<ViewResult>();

        ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
        viewResult.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        viewResult.Model.Should().Be(persons);
    }
}
