using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsService
{
    PersonResponse AddPerson(PersonRequest? personRequest);
    PersonResponse? Get(Guid? id);
    IEnumerable<PersonResponse> GetAll();
}
