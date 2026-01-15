using System.ComponentModel.DataAnnotations;

namespace CitiesManager.DTO;

public class Login
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    [Required]
    public required string Password { get; set; }
}
