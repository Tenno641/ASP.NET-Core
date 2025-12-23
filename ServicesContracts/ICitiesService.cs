namespace ServicesContracts;

public interface ICitiesService
{
    Guid InstanceId { get; }
    IEnumerable<string> GetCities();
}
