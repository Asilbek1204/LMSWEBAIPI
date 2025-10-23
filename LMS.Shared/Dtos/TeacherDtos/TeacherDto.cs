namespace LMS.Shared.Dtos.TeacherDtos
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Bio { get; set; }
        public string Qualifications { get; set; }
        public string Specialization { get; set; }
        public int YearsOfExperience { get; set; }
        public UserDto UserInfo { get; set; }
        public int CourseCount { get; set; }
    }
}