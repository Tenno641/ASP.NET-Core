using System.ComponentModel.DataAnnotations;

namespace AspControllers.CustomValidators;

public class MinimumAgeAttribute : ValidationAttribute
{
    public int MinimumYear { get; set; } = 2000;
    public string DefaultErrorMessage { get; private set; } = "{0} must be smaller than {1}";
    public MinimumAgeAttribute() { }
    public MinimumAgeAttribute(int minimumYear)
    {
        MinimumYear = minimumYear;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return null; 
        int year = (int)value;
        if (year >= 2000) return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, validationContext.DisplayName, MinimumYear));
        return ValidationResult.Success;
    }
}
