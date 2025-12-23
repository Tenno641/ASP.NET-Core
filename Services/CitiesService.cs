using ServicesContracts;

namespace Services;

public class CitiesService : ICitiesService
{
    private readonly IEnumerable<string> _cities;
    public Guid InstanceId { get; private set; }
    public CitiesService()
    {
        InstanceId = Guid.NewGuid();
        _cities =
            [
                "London",
                "Abasyaa",
                "Shafeeq",
                "Ahmed Naser",
                "Adly Manssour"
            ];
    }
    public IEnumerable<string> GetCities()
    {
        return _cities;
    }
}
