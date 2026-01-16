namespace CitiesManager.DTO;

public class UserResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required DateTime Expiration { get; set; }
    public required string Token { get; set; }
}
