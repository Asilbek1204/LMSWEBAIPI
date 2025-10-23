using LMS.Data.Entities;
using LMS.Data.Repositories;
using LMS.Logic.Exceptions;
using LMS.Logic.Helpers;
using LMS.Logic.Services.Interfaces;
using LMS.Shared.Dtos;
using LMS.Shared.Dtos.AuthDtos;

namespace LMS.Logic.Services
{
    public class AuthService(
        IUserRepository userRepository,
        ITeacherRepository teacherRepository,
        IStudentRepository studentRepository,
        IPasswordHasher passwordHasher,
        IJwtHelper jwtHelper) : IAuthService
    {
        // 🧩 Oddiy foydalanuvchini ro‘yxatdan o‘tkazish
        public async Task<LoginResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            await ValidateUserAsync(registerDto.Email, registerDto.UserName);

            var passwordHash = passwordHasher.HashPassword(registerDto.Password);

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                Roles = new List<string> { "User" },
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await userRepository.CreateAsync(user);
            return GenerateLoginResponse(createdUser);
        }

        // 🎓 Student ro‘yxatdan o‘tkazish
        public async Task<LoginResponseDto> RegisterStudentAsync(RegisterStudentDto dto)
        {
            await ValidateUserAsync(dto.Email, dto.UserName);

            var passwordHash = passwordHasher.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Roles = new List<string> { "Student" },
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await userRepository.CreateAsync(user);

            // 🎓 Student profili yaratish
            var studentId = await studentRepository.GenerateStudentIdAsync();
            var student = new Student
            {
                UserId = createdUser.Id,
                StudentId = studentId,
                EnrollmentDate = DateTime.UtcNow
            };
            await studentRepository.CreateAsync(student);

            return GenerateLoginResponse(createdUser);
        }

        // 👨‍🏫 Teacher ro‘yxatdan o‘tkazish
        public async Task<LoginResponseDto> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            await ValidateUserAsync(dto.Email, dto.UserName);

            var passwordHash = passwordHasher.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Roles = new List<string> { "Teacher" },
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await userRepository.CreateAsync(user);

            // 👨‍🏫 Teacher profili yaratish
            var teacher = new Teacher
            {
                UserId = createdUser.Id,
                Bio = dto.Bio ?? "",
                Specialization = dto.Specialization ?? "",
                YearsOfExperience = dto.YearsOfExperience
            };
            await teacherRepository.CreateAsync(teacher);

            return GenerateLoginResponse(createdUser);
        }

        // 🔐 Login funksiyasi (o‘zgarmagan)
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
                throw new NotFoundException("Invalid email or password");

            var isPasswordValid = passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password);
            if (!isPasswordValid)
                throw new BadRequestException("Invalid email or password");

            return GenerateLoginResponse(user);
        }

        // ✅ Foydalanuvchini tekshirish (takror koddan qochish uchun)
        private async Task ValidateUserAsync(string email, string username)
        {
            if (await userRepository.EmailExistsAsync(email))
                throw new ArgumentException("Email already exists");

            if (await userRepository.UserNameExistsAsync(username))
                throw new ArgumentException("Username already exists");
        }

        // 🪙 Token yaratish va javobni qaytarish
        private LoginResponseDto GenerateLoginResponse(User user)
        {
            var token = jwtHelper.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                UserInfo = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = user.Roles,
                    CreatedAt = user.CreatedAt
                }
            };
        }
    }
}
