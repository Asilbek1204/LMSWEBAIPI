namespace LMS.Shared.Dtos.PaginationDtos
{
    public class CourseFilterParams : PaginationParams
    {
        public string? Title { get; set; }
        public Guid? TeacherId { get; set; }
        public int? CategoryId { get; set; }
    }
}
