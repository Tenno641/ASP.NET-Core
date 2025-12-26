using Entities;
using Services.Helpers;
using ServicesContracts.Countries;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using System.ComponentModel.DataAnnotations;

namespace Services.Persons;
public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;
    public PersonsService(ICountriesService countriesService, bool initialize = true)
    {
        _persons = new List<Person>
{
    new() { Id = Guid.NewGuid(), Name = "Ahmed Ali", Email = "ahmed@example.com", DateOfBirth = new DateTime(1990, 5, 12), Gender = "Male", CountryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Address = "Cairo, Egypt", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Sara Hassan", Email = "sara@example.com", DateOfBirth = new DateTime(1992, 8, 20), Gender = "Female", CountryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Address = "Alexandria, Egypt", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "John Smith", Email = "john@example.com", DateOfBirth = new DateTime(1985, 3, 15), Gender = "Male", CountryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Address = "New York, USA", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Emily Clark", Email = "emily@example.com", DateOfBirth = new DateTime(1993, 11, 10), Gender = "Female", CountryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Address = "Los Angeles, USA", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "James Brown", Email = "james@example.com", DateOfBirth = new DateTime(1988, 7, 5), Gender = "Male", CountryId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Address = "Berlin, Germany", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "Olivia Green", Email = "olivia@example.com", DateOfBirth = new DateTime(1995, 1, 22), Gender = "Female", CountryId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Address = "Munich, Germany", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Max Müller", Email = "max@example.com", DateOfBirth = new DateTime(1982, 2, 18), Gender = "Male", CountryId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Address = "London, UK", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Anna Schmidt", Email = "anna@example.com", DateOfBirth = new DateTime(1991, 9, 30), Gender = "Female", CountryId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Address = "Manchester, UK", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "Pierre Dupont", Email = "pierre@example.com", DateOfBirth = new DateTime(1987, 6, 12), Gender = "Male", CountryId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Address = "Paris, France", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Marie Curie", Email = "marie@example.com", DateOfBirth = new DateTime(1990, 12, 7), Gender = "Female", CountryId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Address = "Lyon, France", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Luca Rossi", Email = "luca@example.com", DateOfBirth = new DateTime(1989, 4, 25), Gender = "Male", CountryId = Guid.Parse("66666666-6666-6666-6666-666666666666"), Address = "Rome, Italy", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "Giulia Bianchi", Email = "giulia@example.com", DateOfBirth = new DateTime(1994, 3, 19), Gender = "Female", CountryId = Guid.Parse("66666666-6666-6666-6666-666666666666"), Address = "Milan, Italy", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Carlos Ruiz", Email = "carlos@example.com", DateOfBirth = new DateTime(1986, 10, 8), Gender = "Male", CountryId = Guid.Parse("77777777-7777-7777-7777-777777777777"), Address = "Madrid, Spain", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Sofia Lopez", Email = "sofia@example.com", DateOfBirth = new DateTime(1992, 2, 14), Gender = "Female", CountryId = Guid.Parse("77777777-7777-7777-7777-777777777777"), Address = "Barcelona, Spain", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "Yuki Tanaka", Email = "yuki@example.com", DateOfBirth = new DateTime(1993, 5, 3), Gender = "Female", CountryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Address = "Tokyo, Japan", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Ken Watanabe", Email = "ken@example.com", DateOfBirth = new DateTime(1985, 11, 27), Gender = "Male", CountryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Address = "Osaka, Japan", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Omar Saleh", Email = "omar@example.com", DateOfBirth = new DateTime(1991, 6, 30), Gender = "Male", CountryId = Guid.Parse("12121212-1212-1212-1212-121212121212"), Address = "Riyadh, Saudi Arabia", ReceiveNewsLetter = false },
    new() { Id = Guid.NewGuid(), Name = "Fatima Noor", Email = "fatima@example.com", DateOfBirth = new DateTime(1990, 9, 17), Gender = "Female", CountryId = Guid.Parse("12121212-1212-1212-1212-121212121212"), Address = "Jeddah, Saudi Arabia", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Youssef Amr", Email = "youssef@example.com", DateOfBirth = new DateTime(1988, 12, 2), Gender = "Male", CountryId = Guid.Parse("90909090-9090-9090-9090-909090909090"), Address = "Athens, Greece", ReceiveNewsLetter = true },
    new() { Id = Guid.NewGuid(), Name = "Amina Zayed", Email = "amina@example.com", DateOfBirth = new DateTime(1992, 7, 11), Gender = "Female", CountryId = Guid.Parse("90909090-9090-9090-9090-909090909090"), Address = "Thessaloniki, Greece", ReceiveNewsLetter = false }
};
        _countriesService = countriesService;
    }
    public PersonResponse AddPerson(PersonRequest? personRequest)
    {
        ArgumentNullException.ThrowIfNull(personRequest);
        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(personRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));

        Person person = personRequest.Value.ToPerson();
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

    private IEnumerable<PersonResponse> FilterGeneric<T>(Func<PersonResponse, T> selector, Func<T, bool> predicate)
    {
        return GetAll().Where(person => predicate(selector(person)));
    }

    public IEnumerable<PersonResponse> Filter(string searchBy, string? searchString)
    {
        if (searchString is null) return GetAll();

        return searchBy switch
        {
            "Name" => FilterGeneric(person => person.Name, name => name?.Contains(searchString) ?? true),
            "Email" => FilterGeneric(person => person.Email, email => email?.Contains(searchString) ?? true),
            "DateOfBirth" => FilterGeneric(person => person.DateOfBirth, date => date?.ToString("dd mm yyy").Contains(searchString) ?? true),
            "Age" => FilterGeneric(person => person.Age, age => age?.Equals(searchString) ?? true),
            "Gender" => FilterGeneric(person => person.Gender, gender => gender?.Equals(searchString) ?? true),
            "Country" => FilterGeneric(person => person.CountryName, country => country?.Equals(searchString) ?? true),
            "Address" => FilterGeneric(person => person.Address, address => address?.Contains(searchString) ?? true),
            "ReceiveNewsLetters" => FilterGeneric(person => person.ReceiveNewsLetter, receiveNews => receiveNews.Equals(searchString)),
            _ => GetAll()
        };
    }

    private IEnumerable<PersonResponse> OrderGeneric<T>(IEnumerable<PersonResponse> data, Func<PersonResponse, T> selector, SortOrderOptions sortOrderOptions)
    {
        return sortOrderOptions switch
        {
            SortOrderOptions.Descending => data.OrderByDescending(selector),
            SortOrderOptions.Ascending => data.OrderBy(selector),
            _ => GetAll()
        };
    }

    public IEnumerable<PersonResponse> Order(IEnumerable<PersonResponse> data, string sortBy, SortOrderOptions sortOptions)
    {
        return sortBy switch
        {
            "Name" => OrderGeneric(data, person => person.Name, sortOptions),
            "Email" => OrderGeneric(data, person => person.Email, sortOptions),
            "DateOfBirth" => OrderGeneric(data, person => person.DateOfBirth, sortOptions),
            "Age" => OrderGeneric(data, person => person.Age, sortOptions),
            "Gender" => OrderGeneric(data, person => person.Gender, sortOptions),
            "Country" => OrderGeneric(data, person => person.CountryName, sortOptions),
            "Address" => OrderGeneric(data, person => person.Address, sortOptions),
            "ReceiveNewsLetters" => OrderGeneric(data, person => person.ReceiveNewsLetter, sortOptions),
            _ => GetAll()
        };
    }

    public PersonResponse Update(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        var objectValidation = ValidationHelper.ValidateObject(personUpdateRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.errors.Select(error => error.ErrorMessage)));

        Person? person = _persons.Find(person => person.Id == personUpdateRequest.Value.Id);

        if (person is null) throw new ArgumentException("Not Found Person");

        UpdatePerson(personUpdateRequest.Value, person);

        return ConvertPersonToPersonResponse(person);
    }

    private static void UpdatePerson(PersonUpdateRequest personUpdateRequest, Person person)
    {
        person.Address = personUpdateRequest.Address ?? person.Address;
        person.CountryId = personUpdateRequest.CountryId ?? person.CountryId;
        person.DateOfBirth = personUpdateRequest.DateOfBirth ?? person.DateOfBirth;
        person.Email = personUpdateRequest.Email ?? person.Email;
        person.Gender = personUpdateRequest.Gender.ToString() ?? person.Gender;
        person.Name = personUpdateRequest.Name ?? person.Name;
        person.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;
    }

    public bool Delete(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = _persons.Find(person => person.Id == id);
        if (person is null) return false;

        return _persons.Remove(person);

    }
}
