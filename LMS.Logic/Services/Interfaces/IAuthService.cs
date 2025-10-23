using LMS.Shared.Dtos;
using LMS.Shared.Dtos.AuthDtos;

namespace LMS.Logic.Services.Interfaces
{    public interface IAuthService
    {
        Task<LoginResponseDto> RegisterUserAsync(RegisterDto dto);
        Task<LoginResponseDto> RegisterStudentAsync(RegisterStudentDto dto);
        Task<LoginResponseDto> RegisterTeacherAsync(RegisterTeacherDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
