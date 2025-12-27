using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;

namespace ContactsManager.Controllers;
[Route("[Controller]")]
public class HomeController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    public HomeController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }

    [Route("[action]")]
    public IActionResult Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.Ascending)
    {
        ViewBag.SearchOptions = new Dictionary<string, string>() {
            { "Name", "Person Name"}, {"Email", "Email"}, {"DateOfBirth","Date Of Birth" }, {"Age", "Age"},{"Gender","Gender"}, {"Country", "Gender"},{"Address","Address"}, {"ReceiveNewsLetters", "Receive News Letters" }
        };
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        IEnumerable<PersonResponse> persons = _personsService.GetAll();

        IEnumerable<PersonResponse> filteredPersons = _personsService.Filter(persons, searchBy, searchString);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;

        IEnumerable<PersonResponse> sortedPersons = _personsService.Order(filteredPersons, sortBy, sortOrder);

        return View(sortedPersons);
    }

    [Route("/")]
    public IActionResult Redirect()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    [Route("[action]")]
    [HttpGet]
    public IActionResult CreatePerson()
    {
        ViewBag.Countries = GetCountriesListItem();
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult CreatePerson(PersonRequest personRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = GetCountriesListItem();
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
            return View();
        }
        _personsService.AddPerson(personRequest);
        return RedirectToAction("Index", "Home");
    }

    [Route("[action]/{id:guid}")]
    [HttpGet]
    public IActionResult Update(Guid id)
    {
        PersonResponse? person = _personsService.Get(id);
        if (person is null)
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        IEnumerable<SelectListItem> countries = GetCountriesListItem();
        ViewBag.Countries = countries;

        PersonUpdateRequest personUpdate = person.Value.ToPersonUpdateRequest();
        return View(personUpdate);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult Update(PersonUpdateRequest personUpdateRequest)
    {
        PersonResponse? person = _personsService.Get(personUpdateRequest.Id);
        if (person is null)
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        if (!ModelState.IsValid)
        {
            IEnumerable<SelectListItem> countries = GetCountriesListItem();
            ViewBag.Countries = countries;
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
            return View(personUpdateRequest);
        }

        _personsService.Update(personUpdateRequest);
        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }
    private IEnumerable<SelectListItem> GetCountriesListItem()
    {
        return _countriesService.GetAll().Select(country => new SelectListItem() { Text = country.Name, Value = country.Id.ToString() });
    }

}
