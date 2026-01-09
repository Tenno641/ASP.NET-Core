using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsGetService
{
    Task<PersonResponse?> GetAsync(Guid? id);
    Task<IEnumerable<PersonResponse>> GetAllAsync();
    Task<IEnumerable<PersonResponse>> FilterAsync(string searchBy, string? searchString);
    Task<IEnumerable<PersonResponse>> OrderAsync(IEnumerable<PersonResponse> data, string sortBy, SortOrderOptions sortOptions);
    Task<MemoryStream> GetPersonsCsvAsync();
    Task<MemoryStream> GetPersonsExcelAsync();
}
