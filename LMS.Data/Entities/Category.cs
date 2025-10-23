using System.Collections.Generic;

namespace LMS.Data.Entities
{
    /// Category - bu kurs kategoriyalari
    /// Masalan: "Programming", "Mathematics", "Science"
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// Kategoriyaga tegishli barcha kurslar
        /// One-to-Many: Bir kategoriyada ko'p kurslar bo'lishi mumkin 
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}