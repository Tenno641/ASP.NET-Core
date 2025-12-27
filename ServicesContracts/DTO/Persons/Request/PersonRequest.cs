using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO.Persons.Request;
public record PersonRequest(
    [Required]
    string? Name,
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    string? Email,
    [DataType(DataType.Date)]
    DateTime? DateOfBirth,
    [Required]
    GenderOptions? Gender,
    [Required]
    Guid? CountryId,
    [Required]
    string? Address,
    bool ReceiveNewsLetter
)
{
    public Person ToPerson()
    {
        return new Person()
        {
            Name = Name,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            CountryId = CountryId,
            Address = Address,
            ReceiveNewsLetter = ReceiveNewsLetter
        };
    }
}
