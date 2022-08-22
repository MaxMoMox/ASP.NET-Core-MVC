using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Entities;

public class Student
{
    public int Id { get; set; }

    [Required]
    public int? GroupId { get; set; }

    [Required(ErrorMessage = "Select the course")]
    public int? CourseId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(ErrorMessage = "Name length should be shorter than 50 chars.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(ErrorMessage = "Name length should be shorter than 50 chars.")]

    public string LastName { get; set; } = null!;
    public Group? Group { get; set; }
}