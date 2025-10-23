namespace LMS.Data.Entities
{
    public class User
    {
        /// Userning unique ID si - avtomatik yaratiladi
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// Foydalanuvchi emaili - login qilish uchun ishlatiladi
        public string Email { get; set; }

        /// Foydalanuvchi nomi - ko'rinish uchun
        public string UserName { get; set; }

        /// Parolning hash qilingan versiyasi - hech qachon oddiy parol saqlanmaydi
        public string PasswordHash { get; set; }

        /// Foydalanuvchi ismi
        public string FirstName { get; set; }

        /// Foydalanuvchi familiyasi
        public string LastName { get; set; }

        /// Tug'ilgan sana
        public DateTime DateOfBirth { get; set; }

        /// Qachon yaratilgani
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// Oxirgi marta qachon yangilangan
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