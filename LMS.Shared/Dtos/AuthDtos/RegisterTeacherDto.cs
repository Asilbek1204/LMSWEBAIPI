
namespace LMS.Shared.Dtos.AuthDtos
{
    public class RegisterTeacherDto
    {
        // === User ma’lumotlari ===
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        // === Teacherga xos ma’lumotlar ===
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
