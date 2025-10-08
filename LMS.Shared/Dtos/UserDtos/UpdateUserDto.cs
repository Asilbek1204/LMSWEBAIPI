using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        // Teacher uchun
        public string Bio { get; set; }
        public string Qualifications { get; set; }

        // Student uchun
        public string StudentId { get; set; }
    }
}