using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO.Countries.Request;
public class CountryRequest
{
    [Required]
    public string? Name { get; init; }
    public Country ToCountry()
    {
        return new Country() { Name = Name };
    }
}
