using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AspControllers.Models;

public class Order : IValidatableObject
{
    [BindNever]
    public int? OrderNo { get; set; }
    [Required(ErrorMessage = "OrderDate can't be blank")]
    public DateTime? OrderDate { get; set; }
    [Required]
    public double InvoicePrice { get; set; }
    [Required]
    public List<Product> Products { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OrderDate <= DateTime.Parse("2000-01-01")) yield return new ValidationResult("Order date should be greater than or equal to 2000-01-01.", [nameof(OrderDate)]);

        if (TotalPrice(Products) != InvoicePrice) yield return new ValidationResult("InvoicePrice doesn't match with the total cost of the specified products in the order.", [nameof(InvoicePrice)]);

        PropertyInfo? productsProperty = validationContext.ObjectType.GetProperty(nameof(Products));
        if (productsProperty is not null)
        {
            List<Product>? products = (List<Product>?)productsProperty.GetValue(validationContext.ObjectInstance);
            if (products is not null)
            {
                if (products.Count <= 0) yield return new ValidationResult("Must include prooducts", [nameof(Products)]);   
            }
        }       
    }

    private double TotalPrice(IEnumerable<Product> products)
    {
        return products.Sum(product =>
        {
            if (product.Price is null) return 0;
            return product.Price.Value;
        });
    }

}
