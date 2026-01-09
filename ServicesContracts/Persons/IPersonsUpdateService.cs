using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsUpdateService
{
    Task<PersonResponse> UpdateAsync(PersonUpdateRequest? personUpdateRequest);
}
