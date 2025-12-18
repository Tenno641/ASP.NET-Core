using System.ComponentModel.DataAnnotations;

namespace AspControllers.Models;

public class Person
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public int? Age { get; set; }
}
