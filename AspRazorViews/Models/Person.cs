namespace AspRazorViews.Models;

public class Person
{
    public string? Name { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public Gender? Gender { get; init; }
}

public enum Gender
{
    Male,
    Female
}
