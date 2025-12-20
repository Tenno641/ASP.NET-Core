using AspControllers.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace AspControllers.Models;

public class Person : IValidatableObject
{
    [Required]
    [MinLength(3)]
    public string? Name { get; set; }
    [Required]
    public string? Phone { get; set; }
    [MinimumAge(2000, ErrorMessage = "please {0} must be smaller than {1}")]
    public int? YearOfBirth { get; set; }
    public DateOnly? ToDate { get; init; }
    [DateRange(nameof(ToDate))]
    [Required]
    public DateOnly? FromDate{ get; init; }
    public int? Age { get; set; }
    public string[]? tags { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Age is null && YearOfBirth is null)
        {
            yield return new ValidationResult($"either {nameof(Age)} or {nameof(YearOfBirth)} must be supplied", [nameof(Age), nameof(YearOfBirth)]);
        }
    }
}
