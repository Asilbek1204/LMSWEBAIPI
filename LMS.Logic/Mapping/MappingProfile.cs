using AutoMapper;
using LMS.Data.Entities;
using LMS.Shared.Dtos.StudentDtos;
using LMS.Shared.Dtos.TeacherDtos;
using LMS.Shared.Dtos.UserDtos;

namespace LMS.Logic.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<CreateUserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<UpdateUserDto, TeacherProfile>();


            // Teacher mappings
            CreateMap<TeacherProfile, TeacherDto>();
            CreateMap<CreateUserDto, TeacherProfile>()
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Qualifications, opt => opt.MapFrom(src => src.Qualifications));

            // Student mappings
            CreateMap<StudentProfile, StudentDto>();
            CreateMap<CreateUserDto, StudentProfile>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId));
        }
    }
}