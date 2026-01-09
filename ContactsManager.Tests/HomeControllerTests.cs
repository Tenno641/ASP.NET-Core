using AutoFixture;
using ContactsManager.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Persons;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using Xunit.Abstractions;

namespace ContactsManager.Tests;

public class HomeControllerTests
{
    private readonly ITestOutputHelper _testOutput;
    private readonly Fixture _fixture;
    private readonly Mock<PersonsAddService> _personsAddServiceMock; 
    private readonly Mock<PersonsDeleteService> _personsDeleteServiceMock; 
    private readonly Mock<PersonsGetService> _personsGetServiceMock; 
    private readonly Mock<PersonsUpdateService> _personsUpdateServiceMock; 
    private readonly HomeController _homeController;
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    public HomeControllerTests(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
        _fixture = new Fixture();

        Mock<ICountriesService> countriesServiceMock = new Mock<ICountriesService>();

        _loggerMock = new Mock<ILogger<HomeController>>();

        _personsAddServiceMock = new Mock<PersonsAddService>();
        _personsDeleteServiceMock = new Mock<PersonsDeleteService>();
        _personsGetServiceMock = new Mock<PersonsGetService>();
        _personsUpdateServiceMock = new Mock<PersonsUpdateService>();

        _homeController = new HomeController(countriesServiceMock.Object, _loggerMock.Object, _personsAddServiceMock.Object, _personsUpdateServiceMock.Object, _personsDeleteServiceMock.Object, _personsGetServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_OkResponse()
    {
        // Arrange
        IEnumerable<PersonResponse> persons = _fixture
            .Build<PersonResponse>()
            .CreateMany();

       _personsGetServiceMock 
            .Setup(service => service.FilterAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(persons);

        _personsGetServiceMock
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
