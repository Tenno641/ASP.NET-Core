using System.ComponentModel.DataAnnotations;

namespace AspConfiguration.Models;

public class Person
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public double Height { get; set; }
}

public enum Gender
{
    Male,
    Female
}
