using System.Text.Json.Serialization;

namespace StocksApp.Models;

public readonly record struct StockQuote(
    [property: JsonPropertyName("c")] double CurrentPrice,
    [property: JsonPropertyName("h")] double HighPriceOfDay,
    [property: JsonPropertyName("l")] double LowPriceOfDay,
    [property: JsonPropertyName("o")] double OpenPriceOfDay,
    [property: JsonPropertyName("pc")] double PreviousClosePrice);
