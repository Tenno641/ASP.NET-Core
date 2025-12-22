using AspPartialViews.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspPartialViews.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        ViewData["appTitle"] = "Titly";
        ViewData["listy"] = new List<string>() { "USA", "GERMANY", "FRANCE", "SPAIN", "COLOMBIA" };
        return View();
    }

    [Route("about")]
    public IActionResult About()
    {
        return View();
    }

    [Route("programming-languages")]    
    public IActionResult GetProgrammingLanguages()
    {
        ListModel listModel = new ListModel() { Title = "Programming Languages", Items = ["C#", "TypeScript", "Go", "Rust"] };
        return PartialView("_ListView", listModel);
    }
    
}
