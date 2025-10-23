using System.Collections.Generic;
using System.Reflection;

namespace LMS.Data.Entities
{
    /// Course - bu o'quv kursi
    /// Har bir kurs bir o'qituvchi tomonidan yaratiladi
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        /// Kurs qaysi kategoriyaga tegishli
        public int CategoryId { get; set; }

        /// Kursni qaysi o'qituvchi yaratgan
        /// Teacher jadvalidagi UserId ga bog'lanadi
        public Guid TeacherId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; } = false;

        // Navigation properties
        public virtual Category Category { get; set; }
   
        /// Kursni yaratgan o'qituvchi
        /// Many-to-One: Ko'p kurslar bir o'qituvchiga tegishli bo'lishi mumkin
        public virtual Teacher Teacher { get; set; }

        /// Kursga yozilgan barcha o'quvchilar
        /// One-to-Many: Bir kursga ko'p o'quvchilar yozilishi mumkin
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        /// Kursning barcha modullari (darslari)
        /// One-to-Many: Bir kursda ko'p modullar bo'lishi mumkin
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}