using StocksApp.Models;

namespace StocksApp.ServiceContracts;

public interface IFinnhubService
{
    Task<StockQuote> GetStockQuoteAsync(string stockSymbol);
}
