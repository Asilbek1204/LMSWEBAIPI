using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.Dtos.TeacherDtos
{
    public class UpdateTeacherProfileDto
    {
        /// Teacher profile yangilanishlari
        /// Bio, Qualifications, Specialization, YearsOfExperience
        public UpdateTeacherDto TeacherInfo { get; set; }
    
        /// User ma'lumotlarini yangilash
        /// FirstName, LastName
        public UpdateUserDto UserInfo { get; set; }
    }
}
