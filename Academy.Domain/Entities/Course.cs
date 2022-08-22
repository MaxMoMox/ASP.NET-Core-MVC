using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Entities;

public  class Course
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Course name is required.")]
    [MaxLength(ErrorMessage = "Name length should be shorter than 50 chars.")]
    public string Name { get; set; } = null!;

    [MaxLength(ErrorMessage = "Description length should be shorter than 200 chars.")]
    public string? Description { get; set; }

    public List<Group>? Groups;
}