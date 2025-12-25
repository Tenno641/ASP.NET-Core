using Entities;
using Services.Helpers;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using System.ComponentModel.DataAnnotations;

namespace Services.Persons;
public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;
    public PersonsService(ICountriesService countriesService)
    {
        _persons = [];
        _countriesService = countriesService;
    }
    public PersonResponse AddPerson(PersonRequest? personRequest)
    {
        ArgumentNullException.ThrowIfNull(personRequest);
        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(personRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));

        Person person = personRequest.ToPerson();
        person.Id = Guid.NewGuid();

        _persons.Add(person);

        PersonResponse personResponse = ConvertPersonToPersonResponse(person);

        return personResponse;
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countriesService.Get(personResponse.CountryId)?.Name;
        return personResponse;
    }

    public PersonResponse? Get(Guid? id)
    {
        if (id is null) return null;

        Person? person = _persons.FirstOrDefault(person => person.Id == id);
        if (person is null) return null;

        return ConvertPersonToPersonResponse(person);
    }

    public IEnumerable<PersonResponse> GetAll()
    {
        return _persons.Select(ConvertPersonToPersonResponse);
    }
}
