using Entities;
using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsService
{
    Task<PersonResponse> AddPersonAsync(PersonRequest? personRequest);
    Task<PersonResponse?> GetAsync(Guid? id);
    Task<IEnumerable<PersonResponse>> GetAllAsync();
    Task<IEnumerable<PersonResponse>> FilterAsync(string searchBy, string? searchString);
    Task<IEnumerable<PersonResponse>> OrderAsync(IEnumerable<PersonResponse> data, string sortBy, SortOrderOptions sortOptions);
    Task<PersonResponse> UpdateAsync(PersonUpdateRequest? personUpdateRequest);
    Task<bool> DeleteAsync(Guid? id);
    IEnumerable<Person> GetAllPersonsStoredProcedure();
    void InsertPersonStoredProcedure(Person person);
    Task<MemoryStream> GetPersonsCsvAsync();
    Task<MemoryStream> GetPersonsExcelAsync();
}
