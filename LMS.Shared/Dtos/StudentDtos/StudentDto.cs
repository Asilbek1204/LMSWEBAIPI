using LMS.Shared.Dtos.UserDtos;
using System;

namespace LMS.Shared.Dtos.StudentDtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public UserDto UserInfo { get; set; }
    }
}