using System.ComponentModel.DataAnnotations;

namespace CitiesManager.DTO;

public class Register
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
}
