using AspConfiguration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspDependencyInjection.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;
    private IOptions<Person> _options;
    public HomeController(IConfiguration configuration, IOptions<Person> options)
    {
        _configuration = configuration;
        _options = options;
    }

    [Route("/")]
    public IActionResult Index()
    {
        Person? person = _configuration.GetSection("Person").Get<Person>();
        Person person2 = _options.Value;
        return View(person2);
    }
}
