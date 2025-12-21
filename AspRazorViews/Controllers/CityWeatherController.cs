using AspRazorViews.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace AspRazorViews.Controllers;

public class CityWeatherController : Controller
{
    private readonly ReadOnlyCollection<CityWeather> _cityWeathers = new ReadOnlyCollection<CityWeather>([new() { CityUniqueCode = "LDN", CityName = "London", DateAndTime = DateTime.Parse("2030-01-01 8:00"), TemperatureFahrenheit = 33 }, new() { CityUniqueCode = "NYC", CityName = "London", DateAndTime = DateTime.Parse("2030-01-01 3:00"), TemperatureFahrenheit = 60 }, new() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = DateTime.Parse("2030-01-01 9:00"),  TemperatureFahrenheit = 82
}]);
    [Route("/")]
    public IActionResult Index()
    {
        return View(_cityWeathers); 
    }

    [Route("weather/{cityCode}")]
    public IActionResult CityWeather(string cityCode)
    {
        CityWeather? cityWeather = _cityWeathers.Where(city => city.CityUniqueCode.Equals(cityCode)).FirstOrDefault();
        if (cityWeather is null) return View("Error");
        return View(cityWeather);
    }
}
