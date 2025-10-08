

using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Dtos.TeacherDtos;

public class TeacherCreateDto
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = "";
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Specialization is required for teachers")]
    public string Specialization { get; set; }
}
