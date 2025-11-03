namespace LMS.Data.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        /// Foydalanuvchi roli: Admin, Teacher, Student
        /// Bu property ga qarab qaysi profile yaratilishi belgilanadi
        public ICollection<string> Roles { get; set; } = new List<string>();

        // ==================== NAVIGATION PROPERTIES ====================
        // Bu propertylar Entity Framework ga bog'lanishlarni ko'rsatadi

        /// One-to-One bog'lanish: Bir user faqat bitta teacher profile ga ega bo'ladi
        public virtual Teacher TeacherProfile { get; set; }

        /// One-to-One bog'lanish: Bir user faqat bitta student profile ga ega bo'ladi
        public virtual Student StudentProfile { get; set; }

        /// Constructor - default qiymatlar beradi
        public User()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}