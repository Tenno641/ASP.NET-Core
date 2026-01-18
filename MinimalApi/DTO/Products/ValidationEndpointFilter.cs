
using System.ComponentModel.DataAnnotations;

namespace MinimalApi.DTO.Products;

public class ValidationEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        AddProductRequest? product = context.Arguments.OfType<AddProductRequest>().FirstOrDefault();
        if (product is null) return Results.BadRequest();

        ValidationContext validationContext = new ValidationContext(product);
        ICollection<ValidationResult> validationResults = [];
        bool isValid = Validator.TryValidateObject(product, validationContext, validationResults);

        if (!isValid)
        {
            return Results.BadRequest(validationResults);
        }

        var result = await next(context);

        return result;

    }
}
