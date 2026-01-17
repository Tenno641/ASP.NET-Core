namespace MinimalApi.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }
}
