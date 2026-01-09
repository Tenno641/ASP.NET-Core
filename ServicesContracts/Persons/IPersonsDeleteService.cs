namespace ServicesContracts.Persons;
public interface IPersonsDeleteService
{
    Task<bool> DeleteAsync(Guid? id);
}
