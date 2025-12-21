using AspRazorViews.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspRazorViews.Controllers;

public class HomeController : Controller
{
    [Route("home")]
    public IActionResult Index()
    {
        IEnumerable<Person> persons =
        [
            new(){Name = "Tiesto", Gender = Gender.Male, DateOfBirth = DateTime.Parse("1976-01-22")},
            new() {Name = "Armin Van Buuren", Gender = Gender.Female, DateOfBirth = DateTime.Parse("1980-05-12")}
        ];
        ViewBag.persons = persons;
        ViewBag.appTitle = "Home Page";
        return View();
    }
    [Route("person-details/{name}")]
    public IActionResult Details([FromRoute] string name)
    {
        IEnumerable<Person> persons =
        [
            new(){Name = "Tiesto", Gender = Gender.Male, DateOfBirth = DateTime.Parse("1976-01-22")},
            new() {Name = "Armin Van Buuren", Gender = Gender.Female, DateOfBirth = DateTime.Parse("1980-05-12")}
        ];
        Person? person = persons.Where(person => person.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        ViewBag.appTitle = "Person Details";
        return View(person);
    }
    [Route("person-products")]
    public IActionResult PersonWithProducts()
    {
        Person person = new() { Name = "Avira", Gender = Gender.Male, DateOfBirth = DateTime.Parse("1995-07-16") };
        Product product = new() { ProductId = 2, ProductName = "DDJ 300 Deck-Mixer" };

        PersonProducts personProducts = new() { Person = person, Product = product };
        return View(personProducts);
    }
} 