using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Dtos.StudentDtos;

public class StudentUpdateDto
{
    [Required, MaxLength(30)]
    public string FirstName { get; set; } = null!;
    [Required, MaxLength(30)]
    public string LastName { get; set; } = null!;
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
}
