using Microsoft.AspNetCore.Mvc;
using StocksApp.Models;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers;

public class HomeController : Controller
{
    private IFinnhubService _finnhubService;
    public HomeController(IFinnhubService finnhubService)
    {
        _finnhubService = finnhubService;
    }
    [Route("/")]
    public async Task<IActionResult> Index()
    {
        string stockSymbol = "MSFT";
        StockQuote stockQuote = await _finnhubService.GetStockQuoteAsync(stockSymbol);
        return View(stockQuote);
    }
}
