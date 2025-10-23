using AutoMapper;
using LMS.Data.Entities;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.AuthDtos;
using LMS.Shared.Dtos.EntityDtos;
using LMS.Shared.Dtos.StudentDtos;
using LMS.Shared.Dtos.TeacherDtos;

namespace LMS.Logic.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            // 🧍‍♂️ Oddiy User
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Parolni hashlaymiz keyinroq

            // Student mappings
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count));
             

            CreateMap<UpdateStudentDto, Student>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterStudentDto, User>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => new List<string> { "Student" }))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<RegisterStudentDto, Student>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.StudentId) ?
                    $"STU{DateTime.UtcNow:yyyyMMddHHmmss}" : src.StudentId));

            // Teacher mappings
            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count));

            CreateMap<UpdateTeacherDto, Teacher>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterTeacherDto, User>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => new List<string> { "Teacher" }))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<RegisterTeacherDto, Teacher>();

            // Category mappings
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryDto>();

            //// Course mappings
            //CreateMap<CourseCreateDto, Course>();
            //CreateMap<CourseUpdateDto, Course>();
            //CreateMap<Course, CourseDto>()
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            //    .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
            //        src.Teacher.User.FirstName + " " + src.Teacher.User.LastName))
            //    .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count))
            //    .ForMember(dest => dest.ModuleCount, opt => opt.MapFrom(src => src.Modules.Count));

            // Course mappings
            CreateMap<CourseCreateDto, Course>()
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore()) // TeacherId mapping qilinmaydi
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<CourseUpdateDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src =>
                    src.Teacher.User.FirstName + " " + src.Teacher.User.LastName))
                .ForMember(dest => dest.EnrollmentCount, opt => opt.MapFrom(src => src.Enrollments.Count))
                .ForMember(dest => dest.ModuleCount, opt => opt.MapFrom(src => src.Modules.Count));

            // Module mappings
            CreateMap<ModuleCreateDto, Module>();
            CreateMap<ModuleUpdateDto, Module>();
            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));

            // Enrollment mappings
            CreateMap<EnrollmentCreateDto, Enrollment>();
            CreateMap<EnrollmentUpdateDto, Enrollment>();
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src =>
                    src.Student.User.FirstName + " " + src.Student.User.LastName));

        }
    }
}