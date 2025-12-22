using AspViewComponents.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspViewComponents.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("about")]
    public IActionResult About()
    {
        return View();
    }

    [Route("load-people")]
    public IActionResult LoadPeople()
    {
        PersonGrid model = new PersonGrid()
        {
            GridTitle = "Person",
            Persons =
            [
                new Person() {Name = "Ibrahim", JoBTitle = "Captian"},
            new Person() {Name = "Mostafa", JoBTitle = "Theif"},
            new Person() {Name = "Hussien", JoBTitle = "Depressed" }
            ]
        };
        return ViewComponent("Grid", new { grid = model });
    }
}
