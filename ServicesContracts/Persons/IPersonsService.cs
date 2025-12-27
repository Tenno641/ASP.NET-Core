using ServicesContracts.DTO.Persons;
using ServicesContracts.DTO.Persons.Request;
using ServicesContracts.DTO.Persons.Response;

namespace ServicesContracts.Persons;
public interface IPersonsService
{
    PersonResponse AddPerson(PersonRequest? personRequest);
    PersonResponse? Get(Guid? id);
    IEnumerable<PersonResponse> GetAll();
    IEnumerable<PersonResponse> Filter(IEnumerable<PersonResponse> data, string searchBy, string? searchString);
    IEnumerable<PersonResponse> Order(IEnumerable<PersonResponse> data, string sortBy, SortOrderOptions sortOptions);
    PersonResponse Update(PersonUpdateRequest? personUpdateRequest);
    bool Delete(Guid? id);

}
