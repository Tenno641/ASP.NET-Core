using Microsoft.AspNetCore.Mvc;

namespace AspRazorViews.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
