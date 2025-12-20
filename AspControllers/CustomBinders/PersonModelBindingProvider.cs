using AspControllers.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AspControllers.CustomBinders;

public class PersonModelBindingProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Metadata.BinderType != typeof(Person)) return null;
        return new BinderTypeModelBinder(typeof(PersonModelBinder));
    }
}
