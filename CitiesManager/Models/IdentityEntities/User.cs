using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Models.IdentityEntities;

public class User : IdentityUser<Guid>
{
    public User()
    {
        Id = Guid.CreateVersion7();
    }
    public required string Name { get; init; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
