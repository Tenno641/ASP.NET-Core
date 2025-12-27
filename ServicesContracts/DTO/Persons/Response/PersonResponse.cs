using Entities;
using ServicesContracts.DTO.Persons.Request;

namespace ServicesContracts.DTO.Persons.Response;
public record struct PersonResponse
(
    Guid Id,
    string? Name,
    string? Email,
    DateTime? DateOfBirth,
    string? Gender,
    Guid? CountryId,
    string? CountryName,
    int? Age,
    string? Address,
    bool ReceiveNewsLetter
);

public static class PersonResponseExtension
{
    public static PersonResponse ToPersonResponse(this Person person) {
        return new PersonResponse()
        {
            Id = person.Id,
            Name = person.Name,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryId = person.CountryId,
            Address = person.Address,
            ReceiveNewsLetter = person.ReceiveNewsLetter,
            Age = CalculateAge(person.DateOfBirth)
        };
    }

    public static PersonUpdateRequest ToPersonUpdateRequest(this PersonResponse personResponse)
    {
        return new PersonUpdateRequest()
        {
            Id = personResponse.Id,
            Address = personResponse.Address,
            CountryId = personResponse.CountryId,
            DateOfBirth = personResponse.DateOfBirth,
            Email = personResponse.Email,
            Gender = Enum.TryParse<GenderOptions>(personResponse.Gender, true, out GenderOptions value) ? value : null,
            Name = personResponse.Name,
            ReceiveNewsLetter = personResponse.ReceiveNewsLetter
        };
    }

    private static int? CalculateAge(DateTime? dateTime)
    {
        return (int?)(DateTime.Now - dateTime)?.TotalDays / 365;
    }
}
