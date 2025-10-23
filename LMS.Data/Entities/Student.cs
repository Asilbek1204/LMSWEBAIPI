using System.Collections.Generic;

namespace LMS.Data.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}