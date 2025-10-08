using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.Entities
{
    public class TeacherProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Bio { get; set; }
        public string Qualifications { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}