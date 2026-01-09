using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsAddService
{
    Task<PersonResponse> AddPersonAsync(PersonRequest? personRequest);
    Task<IEnumerable<PersonResponse>> AddRangeAsync(IEnumerable<PersonRequest> persons);
}
