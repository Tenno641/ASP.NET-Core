using Entities;

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
    public static PersonResponse ToPersonResponse(this Person person)
    {
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
    private static int? CalculateAge(DateTime? dateTime)
    {
        return (int?)(DateTime.Now - dateTime)?.TotalDays / 365;
    }
}
