using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data.Entities
{
    public class StudentProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        //public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}