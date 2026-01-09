using Entities;
using RepositoryContracts;
using Services.Helpers;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;

namespace Services.Persons;
public class PersonsUpdateService : IPersonsUpdateService
{
    private readonly IPersonsRepository _repository;
    public PersonsUpdateService(IPersonsRepository repository)
    {
        _repository = repository;
    }
    public async Task<PersonResponse> UpdateAsync(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        var objectValidation = ValidationHelper.ValidateObject(personUpdateRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.errors.Select(error => error.ErrorMessage)));

        Person? person = await _repository.GetAsync(personUpdateRequest.Id);

        if (person is null) throw new ArgumentException("Not Found Person");

        await _repository.UpdateAsync(personUpdateRequest.ToPerson());

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
}
