using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Models.IdentityEntities;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
        Id = Guid.CreateVersion7();
    }
}
