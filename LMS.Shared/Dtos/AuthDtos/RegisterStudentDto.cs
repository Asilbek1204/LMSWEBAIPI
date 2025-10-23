namespace LMS.Shared.Dtos.AuthDtos
{
    public class RegisterStudentDto
    {
        // === User ma’lumotlari ===
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        // === Studentga xos ma’lumotlar ===
        public string? StudentId { get; set; }  // optional — tizimda avtomatik ham yaratiladi
    }
}
