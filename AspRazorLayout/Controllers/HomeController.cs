using Microsoft.AspNetCore.Mvc;

namespace AspRazorLayout.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("about-company")]
    public IActionResult About()
    {
        return View();
    }

    [Route("contact-company")]
    public IActionResult Contact()
    {
        return View();
    }
}
