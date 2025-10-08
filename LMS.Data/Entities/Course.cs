namespace LMS.Data.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string TeacherId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; } // soatda
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; } = false;

        // Navigation properties
        //public virtual Category Category { get; set; }
        public virtual TeacherProfile Teacher { get; set; }
        //public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        //public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}