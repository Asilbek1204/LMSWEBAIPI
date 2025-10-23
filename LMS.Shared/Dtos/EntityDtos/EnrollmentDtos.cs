using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.Dtos.EntityDtos
{
    public class EnrollmentCreateDto
    {
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
    }

    public class EnrollmentUpdateDto
    {
        public string Status { get; set; } = "Active";
    }

    public class EnrollmentDto
    {
        public int Id { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active";
    }
}
