using System.Collections.Generic;

namespace LMS.Data.Entities
{
    /// Teacher - bu o'qituvchi profili
    /// Faqat Role="Teacher" bo'lgan userlar uchun yaratiladi
    public class Teacher
    {
        /// Teacher ning ID si
   
        public Guid Id { get; set; }

        /// Qaysi user bu teacher profili egasi
        /// User jadvalidagi Id ga bog'lanadi
        public string UserId { get; set; }

        /// O'qituvchi haqida qisqa ma'lumot
        public string Bio { get; set; }
   
        /// O'qituvchi ixtisosligi (masalan: "Mathematics, Physics")
        public string Specialization { get; set; }

        /// Tajriba yillari
        public int YearsOfExperience { get; set; }

        // ==================== NAVIGATION PROPERTIES ====================
   
        /// Teacher profile qaysi user ga tegishli
        /// One-to-One bog'lanishning boshqa tomoni
        public virtual User User { get; set; }

        /// O'qituvchi qaysi kurslarni o'qitadi
        /// One-to-Many bog'lanish: Bir o'qituvchi ko'p kurslar yaratishi mumkin
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}