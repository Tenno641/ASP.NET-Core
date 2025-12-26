using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServicesContracts.DTO.Persons.Request;

public record struct PersonUpdateRequest(
    [property: Required]
    Guid Id,
    [property: Required]
    string? Name,
    [property: Required]
    [property: EmailAddress]
    string? Email,
    DateTime? DateOfBirth,
    GenderOptions? Gender,
    Guid? CountryId,
    string? Address,
    bool ReceiveNewsLetter
    )
{
    public Person ToPerson()
    {
        return new Person()
        {
            Id = Id,
            Name = Name,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            CountryId = CountryId,
            Address = Address,
            ReceiveNewsLetter = ReceiveNewsLetter
        };
    }
};
