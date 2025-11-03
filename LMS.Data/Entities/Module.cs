namespace LMS.Data.Entities
{
    /// Module - bu kursning bir bo'limi (darsi)
    /// Har bir kurs bir necha modullardan tashkil topadi
    public class Module
    {
        public int Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// Modul kontenti (matn, video link, fayl yo'li)
        public string Content { get; set; }
        /// Modul tartib raqami (1, 2, 3...)
        public int Order { get; set; }
        /// Many-to-One: Ko'p modullar bir kursga tegishli bo'lishi mumkin
        public virtual Course Course { get; set; }
    }
}