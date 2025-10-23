namespace LMS.Data.Entities
{
    /// Enrollment - bu o'quvchining kursga yozilishi
    /// Bu jadval o'quvchi va kurs o'rtasidagi bog'lovchi vazifasini bajaradi
    public class Enrollment
    {
        public int Id { get; set; }

        /// Qaysi kursga yozilgan
        public Guid CourseId { get; set; }

        /// Qaysi o'quvchi yozilgan (Student jadvalidagi Id)
        public Guid StudentId { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        /// Yozilish holati: Active, Completed, Dropped
        public string Status { get; set; } = "Active";

        // Navigation properties
        /// Yozilgan kurs
        /// Many-to-One: Ko'p yozilishlar bir kursga bo'lishi mumkin
        public virtual Course Course { get; set; }

        /// Yozilgan o'quvchi
        /// Many-to-One: Ko'p yozilishlar bir o'quvchiga tegishli bo'lishi mumkin
   
        public virtual Student Student { get; set; }
    }
}