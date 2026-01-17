namespace CitiesManager.DTO;

public class AuthenticationToken
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
