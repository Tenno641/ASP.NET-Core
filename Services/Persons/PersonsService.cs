using Entities;
using Entities.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;
using ServicesContracts.Persons;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Services.Persons;
public class PersonsService : IPersonsService
{
    private readonly PersonsDbContext _dbContext;
    public PersonsService(PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PersonResponse> AddPersonAsync(PersonRequest? personRequest)
    {
        ArgumentNullException.ThrowIfNull(personRequest);
        (bool isValid, IReadOnlyCollection<ValidationResult> validationResults) objectValidation = ValidationHelper.ValidateObject(personRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.validationResults.Select(result => result.ErrorMessage)));

        Person person = personRequest.ToPerson();
        person.Id = Guid.NewGuid();

        _dbContext.Persons.Add(person);
        await _dbContext.SaveChangesAsync();
        //InsertPersonStoredProcedure(person);

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<PersonResponse?> GetAsync(Guid? id)
    {
        if (id is null) return null;

        return await _dbContext.Persons
            .Include(person => person.Country)
            .AsNoTracking()
            .Where(person => person.Id == id)
            .Select(PersonResponseExtension.ToPersonResponse)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<PersonResponse>> GetAllAsync()
    {
        return await _dbContext.Persons.Include(person => person.Country).Select(PersonResponseExtension.ToPersonResponse).ToListAsync();
    }
    public async Task<IEnumerable<PersonResponse>> FilterAsync(string searchBy, string? searchString)
    {
        if (searchString is null) return await GetAllAsync();

        return searchBy switch
        {
            "Name" => await FilterGenericAsync(person => person.Name, name => name?.Contains(searchString) ?? true),
            "Email" => await FilterGenericAsync(person => person.Email, email => email?.Contains(searchString) ?? true),
            "DateOfBirth" => await FilterGenericAsync(person => person.DateOfBirth, date => date?.ToString("dd mm yyy").Contains(searchString) ?? true),
            "Age" => await FilterGenericAsync(person => person.Age, age => age?.Equals(searchString) ?? true),
            "Gender" => await FilterGenericAsync(person => person.Gender, gender => gender?.Equals(searchString) ?? true),
            "Country" => await FilterGenericAsync(person => person.CountryName, country => country?.Equals(searchString) ?? true),
            "Address" => await FilterGenericAsync(person => person.Address, address => address?.Contains(searchString) ?? true),
            "ReceiveNewsLetters" => await FilterGenericAsync(person => person.ReceiveNewsLetter, receiveNews => receiveNews.Equals(searchString)),
            _ => await GetAllAsync()
        };
    }
    public async Task<IEnumerable<PersonResponse>> OrderAsync(string sortBy, SortOrderOptions sortOptions)
    {
        return sortBy switch
        {
            "Name" => await OrderGenericAsync(person => person.Name, sortOptions),
            "Email" => await OrderGenericAsync(person => person.Email, sortOptions),
            "DateOfBirth" => await OrderGenericAsync(person => person.DateOfBirth, sortOptions),
            "Age" => await OrderGenericAsync(person => person.Age, sortOptions),
            "Gender" => await OrderGenericAsync(person => person.Gender, sortOptions),
            "Country" => await OrderGenericAsync(person => person.CountryName, sortOptions),
            "Address" => await OrderGenericAsync(person => person.Address, sortOptions),
            "ReceiveNewsLetters" => await OrderGenericAsync(person => person.ReceiveNewsLetter, sortOptions),
            _ => await GetAllAsync()
        };
    }
    public async Task<PersonResponse> UpdateAsync(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        var objectValidation = ValidationHelper.ValidateObject(personUpdateRequest);

        if (!objectValidation.isValid) throw new ArgumentException(string.Join(",", objectValidation.errors.Select(error => error.ErrorMessage)));

        Person? person = _dbContext.Persons.FirstOrDefault(person => person.Id == personUpdateRequest.Id);

        if (person is null) throw new ArgumentException("Not Found Person");

        UpdatePerson(personUpdateRequest, person);
        await _dbContext.SaveChangesAsync();

        return PersonResponseExtension.ToPersonResponse.Compile().Invoke(person);
    }
    public async Task<bool> DeleteAsync(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _dbContext.Persons.FirstOrDefaultAsync(person => person.Id == id);
        if (person is null) return false;

        _dbContext.Remove(person);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    public IEnumerable<Person> GetAllPersonsStoredProcedure()
    {
        FormattableString query = FormattableStringFactory.Create("EXECUTE [dbo].[PersonsGet]");
        return _dbContext.Persons.FromSql(query);
    }
    public void InsertPersonStoredProcedure(Person person)
    {
        SqlParameter[] sqlParameters = new SqlParameter[]
        {
            new("@Id", person.Id),
            new("@Name", person.Name),
            new("@Email", person.Email),
            new("@DateOfBirth", person.DateOfBirth),
            new("@Gender", person.Gender),
            new("@CountryId", person.CountryId),
            new("@Address", person.Address),
            new("@ReceiveNewsLetter", person.ReceiveNewsLetter)
        };
        _dbContext.Database.ExecuteSqlRaw("EXECUTE [dbo].[PersonInsert] @Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetter", sqlParameters);
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
    private async Task<IEnumerable<PersonResponse>> OrderGenericAsync<T>(Func<PersonResponse, T> selector, SortOrderOptions sortOrderOptions)
    {
        return sortOrderOptions switch
        {
            SortOrderOptions.Descending => _dbContext.Persons.Select(PersonResponseExtension.ToPersonResponse).OrderByDescending(selector),
            SortOrderOptions.Ascending => _dbContext.Persons.Select(PersonResponseExtension.ToPersonResponse).OrderBy(selector),
            _ => await GetAllAsync()
        };
    }
    private async Task<IEnumerable<PersonResponse>> FilterGenericAsync<T>(Func<PersonResponse, T> selector, Func<T, bool> predicate)
    {
        return await _dbContext.Persons.Select(PersonResponseExtension.ToPersonResponse).Where(person => predicate(selector(person))).ToListAsync();
    }
}
