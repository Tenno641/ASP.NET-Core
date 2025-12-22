namespace AspViewComponents.Models;

public class PersonGrid
{
    public string GridTitle { get; init; }
    public IEnumerable<Person> Persons { get; init; }
}
