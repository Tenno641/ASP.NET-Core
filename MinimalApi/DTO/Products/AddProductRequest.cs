using System.ComponentModel.DataAnnotations;

namespace MinimalApi.DTO.Products;

public class AddProductRequest
{
    [Required(ErrorMessage = "Please provide product's name")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Please provide product's price")]
    public required decimal Price { get; set; }
    [MaxLength(20)]
    [Required]
    public required string SerialNumber { get; set; }
}
