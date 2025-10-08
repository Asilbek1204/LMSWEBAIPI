

using Microsoft.AspNetCore.Identity;

namespace LMS.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual TeacherProfile TeacherProfile { get; set; }
        public virtual StudentProfile StudentProfile { get; set; }
    }
}