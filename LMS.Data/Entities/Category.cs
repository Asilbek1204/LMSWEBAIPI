using System.Collections.Generic;

namespace LMS.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// Category - Course One-to-Many:
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}