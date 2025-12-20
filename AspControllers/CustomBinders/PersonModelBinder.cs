using AspControllers.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspControllers.CustomBinders;

public class PersonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        Person person = new Person();
        person.Name = bindingContext.ValueProvider.GetValue("firstName").FirstValue;
        person.Name += bindingContext.ValueProvider.GetValue("lastName").FirstValue;
        person.Phone = bindingContext.ValueProvider.GetValue("phone").FirstValue;
        person.YearOfBirth = Convert.ToInt32(bindingContext.ValueProvider.GetValue("yearOfBirth").FirstValue);
        bindingContext.Result = ModelBindingResult.Success(person);
        return Task.CompletedTask;
    }
}
