using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AspControllers.CustomValidators;

public class DateRangeAttribute : ValidationAttribute
{
    private readonly string _defaultErrorMessage = "{1} can't be earlier than {0}";
    private readonly string _otherProperty;
    public DateRangeAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly fromDate) return new ValidationResult("Value is invalid");

        PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(_otherProperty);
        if (otherProperty?.GetValue(validationContext.ObjectInstance) is not DateOnly toDate) return new ValidationResult("Value is invalid");

        if (fromDate <= toDate) return ValidationResult.Success;
        return new ValidationResult(string.Format(ErrorMessage ?? _defaultErrorMessage, validationContext.DisplayName, _otherProperty));
    }
}
