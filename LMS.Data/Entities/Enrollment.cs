namespace LMS.Data.Entities
{
    /// Enrollment - bu o'quvchining kursga yozilishi,Bu jadval o'quvchi va kurs o'rtasidagi bog'lovchi vazifasini bajaradi
    public class Enrollment
    {
        public int Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        /// Yozilish holati: Active, Completed, Dropped
        public string Status { get; set; } = "Active";

        // Navigation properties
        /// Many-to-One: Ko'p yozilishlar bir kursga bo'lishi mumkin
        public virtual Course Course { get; set; }
        /// Many-to-One: Ko'p yozilishlar bir o'quvchiga tegishli bo'lishi mumkin
        public virtual Student Student { get; set; }
    }
}