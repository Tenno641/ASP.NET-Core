using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativaio.AspNetCore;
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
    public async Task<IActionResult> Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.Ascending)
    {
        ViewBag.SearchOptions = new Dictionary<string, string>() {
            { "Name", "Person Name"}, {"Email", "Email"}, {"DateOfBirth","Date Of Birth" }, {"Age", "Age"},{"Gender","Gender"}, {"CountryName", "Country"},{"Address","Address"}, {"ReceiveNewsLetters", "Receive News Letters" }
        };

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        IEnumerable<PersonResponse> filteredPersons = await _personsService.FilterAsync(searchBy, searchString);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;

        IEnumerable<PersonResponse> sortedPersons = await _personsService.OrderAsync(filteredPersons, sortBy, sortOrder);

        return View(sortedPersons);
    }

    [Route("/")]
    public IActionResult Redirect()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> CreatePerson()
    {
        ViewBag.Countries = await GetCountriesListItemsAsync();
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> CreatePerson(PersonRequest personRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = GetCountriesListItemsAsync();
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
            return View();
        }
        await _personsService.AddPersonAsync(personRequest);
        return RedirectToAction("Index", "Home");
    }

    [Route("[action]/{id:guid}")]
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        PersonResponse? person = await _personsService.GetAsync(id);
        if (person is null)
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
        IEnumerable<SelectListItem> countries = await GetCountriesListItemsAsync();
        ViewBag.Countries = countries;

        PersonUpdateRequest personUpdate = person.Value.ToPersonUpdateRequest();
        return View(personUpdate);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Update(PersonUpdateRequest personUpdateRequest)
    {
        PersonResponse? person = await _personsService.GetAsync(personUpdateRequest.Id);
        if (person is null)
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        if (!ModelState.IsValid)
        {
            IEnumerable<SelectListItem> countries = await GetCountriesListItemsAsync();
            ViewBag.Countries = countries;
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
            ModelState.Clear();
            return View(person.Value.ToPersonUpdateRequest());
        }

        await _personsService.UpdateAsync(personUpdateRequest);
        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [Route("[action]/{id:guid}")]
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        PersonResponse? person = await _personsService.GetAsync(id);
        if (person is null)
        {
            return RedirectToAction(actionName: "Index");
        }
        return View(person);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
    {
        await _personsService.DeleteAsync(personUpdateRequest.Id);
        return RedirectToAction(actionName: "Index");
    }
    [Route("PersonsPdf")]
    public async Task<IActionResult> PersonsPdf()
    {
        IEnumerable<PersonResponse> persons = await _personsService.GetAllAsync();
        return new ViewAsPdf(persons)
        {
            PageOrientation = Orientation.Landscape
        };
    }

    [Route("PersonsCsv")]
    public async Task<IActionResult> PersonsCsv()
    {
        MemoryStream memoryStream = await _personsService.GetPersonsCsvAsync();
        return File(memoryStream, "text/csv");
    }
    [Route("PersonsExcel")]
    public async Task<IActionResult> PersonsExcel()
    {
        MemoryStream stream = await _personsService.GetPersonsExcelAsync();
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Persons.xlsx");
    }
    private async Task<IEnumerable<SelectListItem>> GetCountriesListItemsAsync()
    {
        return (await _countriesService.GetAllAsync()).Select(country => new SelectListItem() { Text = country.Name, Value = country.Id.ToString() });
    }
}
