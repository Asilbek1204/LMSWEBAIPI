using System.Collections.Generic;

namespace LMS.Data.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }

        // navigation properties
        /// One-to-One bog'lanishning boshqa tomoni
        public virtual User User { get; set; }

        /// One-to-Many bog'lanish: Bir o'qituvchi ko'p kurslar yaratishi mumkin
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}