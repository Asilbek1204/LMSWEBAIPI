using LMS.Logic.Exceptions;
using LMS.Logic.Services;
using LMS.Shared.Dtos.StudentDtos;
using LMS.Shared.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly ILogger<StudentsController> logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            this.studentService = studentService;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
           
                Console.WriteLine("-->Ishlayapti..............................");
                var students = await studentService.GetAllStudentsAsync();
                return Ok(students);
           
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
              var student = await studentService.GetStudentByIdAsync(id);
                return Ok(student);
      
        }

        [HttpGet("user/{userId}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<StudentDto>> GetStudentByUserId(string userId)
        {
             var student = await studentService.GetStudentByUserIdAsync(userId);
                return Ok(student);
            
          
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentDto>> CreateStudent(CreateUserDto createUserDto)
        {
 
          var student = await studentService.CreateStudentAsync(createUserDto);
          return Ok(student);

        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<StudentDto>> UpdateStudent(int id, UpdateUserDto updateUserDto)
        {
            //var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            //var isAdmin = User.IsInRole("Admin");

            //if (!isAdmin)
            //{
            //    var existingStudent = await studentService.GetStudentByIdAsync(id);
            //if (existingStudent.UserInfo.Id != currentUserId)
            //     throw new BadRequestException(message: "Only Admin and Student can update this student.");
            //}

            var student = await studentService.UpdateStudentAsync(id, updateUserDto);
                return Ok(student);
          
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
         
                await studentService.DeleteStudentAsync(id);
                return NoContent();
            
         
        }
    }
}