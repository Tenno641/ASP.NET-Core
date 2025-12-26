using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;

namespace ContactsManager.Controllers;
public class HomeController : Controller
{
    private readonly IPersonsService _personsService;
    public HomeController(IPersonsService personsService)
    {
        _personsService = personsService;
    }

    [Route("persons/index")]
    public IActionResult Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.Ascending)
    {
        ViewBag.SearchOptions = new Dictionary<string, string>() {
            { "Name", "Person Name"}, {"Email", "Email"}, {"DateOfBirth","Date Of Birth" }, {"Age", "Age"},{"Gender","Gender"}, {"Country", "Gender"},{"Address","Address"}, {"ReceiveNewsLetters", "Receive News Letters" }
        };
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        IEnumerable<PersonResponse> FilteredPersons = _personsService.Filter(searchBy, searchString);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;

        IEnumerable<PersonResponse> SortedPersons = _personsService.Order(FilteredPersons, sortBy, sortOrder);

        return View(SortedPersons);
    }

    [Route("/")]
    public IActionResult Redirect()
    {
        return RedirectToAction(nameof(Index), "Home");
    }
}
