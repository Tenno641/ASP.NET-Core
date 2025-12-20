using System.ComponentModel.DataAnnotations;

namespace AspControllers.Models;

public class Product
{
    [Required]
    public int? ProductCode { get; set; }
    [Required]
    public double? Price { get; set; }
    [Required]
    public int? Quantity { get; set; }
}
