

namespace LMS.Shared.Dtos.StudentDtos
{
    public class UpdateStudentProfileDto
    {
        /// Student profile yangilanishlari
        public UpdateStudentDto StudentInfo { get; set; }

        /// User ma'lumotlarini yangilash
        public UpdateUserDto UserInfo { get; set; }
    }
}
