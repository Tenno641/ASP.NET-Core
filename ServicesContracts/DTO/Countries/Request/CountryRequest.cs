using Entities;
using System.ComponentModel.DataAnnotations;

public class CountryRequest
{
    [Required]
    public string? Name { get; init; }
    public Country ToCountry()
    {
        return new Country() { Name = this.Name };
    }
}
