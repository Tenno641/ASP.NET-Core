using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Models;

public class City
{
    [Key]
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string? Name { get; set; }
}
