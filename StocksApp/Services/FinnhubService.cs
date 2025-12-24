using StocksApp.Models;
using StocksApp.ServiceContracts;
using System.Text.Json;

namespace StocksApp.Services;

public class FinnhubService : IFinnhubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    public async Task<StockQuote> GetStockQuoteAsync(string stockSymbol)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        HttpRequestMessage requestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
            Method = HttpMethod.Get
        };

        HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
        responseMessage.EnsureSuccessStatusCode();

        Stream stream = responseMessage.Content.ReadAsStream();

        StreamReader streamReader = new StreamReader(stream);
        string content = streamReader.ReadToEnd();

        /*
         * 
         * OR
         * string content = await httpClient
            .GetStringAsync(new Uri($"/quote?symbol={stockSymbol}&token=${_configuration
            .GetValue<string>("FinnhubToken")}")); 
         * 
         * **/
        StockQuote? stockQuote = JsonSerializer.Deserialize<StockQuote>(content);
        return stockQuote ?? new StockQuote();
    }
}
