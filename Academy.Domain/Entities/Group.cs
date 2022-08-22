using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Entities;

public class Group
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Select the course.")]
    public int? CourseId { get; set; }

    [Required(ErrorMessage = "Group name is required.")]
    [MaxLength(ErrorMessage = "Name length should be shorter than 50 chars.")]
    public string Name { get; set; } = null!;

    public Course? Course { get; set; }
    public List<Student>? Students { get; set; }
}