using Microsoft.AspNetCore.Mvc;
using ServicesContracts;

namespace AspDependencyInjection.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _cityService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public HomeController(ICitiesService citiesService, IServiceScopeFactory serviceScopeFactory)
    {
        _cityService = citiesService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    [Route("/")]
    public IActionResult Index()
    {
        IEnumerable<string> cities = _cityService.GetCities();
        ViewBag.ServiceInstanceId = _cityService.InstanceId;
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
            ViewBag.CustomServiceInstanceId = citiesService.InstanceId;
        }
        return View(cities);
    }
}
