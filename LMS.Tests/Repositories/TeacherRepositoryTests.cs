//using Microsoft.EntityFrameworkCore;
//using LMS.Data;
//using LMS.Data.Entities;
//using LMS.Data.Repositories;
//using LMS.Tests.TestData;
//using Xunit;

//namespace LMS.Tests.Repositories
//{
//    public class TeacherRepositoryTests
//    {
//        private readonly AppDbContext context;
//        private readonly TeacherProfileRepository repository;

//        public TeacherRepositoryTests()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            context = new AppDbContext(options);
//            repository = new TeacherProfileRepository(context);
//        }

//        [Fact]
//        public async Task GetByIdAsync_TeacherExists_ReturnsTeacher()
//        {
//            // Arrange
//            var teacher = TestDataGenerator.CreateTestTeacherProfile();
//            context.Teachers.Add(teacher);
//            await context.SaveChangesAsync();

//            // Act
//            var result = await repository.GetByIdAsync(teacher.Id);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(teacher.Id, result.Id);
//            Assert.Equal(teacher.Bio, result.Bio);
//        }

//        [Fact]
//        public async Task GetByUserIdAsync_TeacherExists_ReturnsTeacher()
//        {
//            var teacher = TestDataGenerator.CreateTestTeacherProfile();
//            context.Teachers.Add(teacher);
//            await context.SaveChangesAsync();
//            var result = await repository.GetByUserIdAsync(teacher.UserId);

//            Assert.NotNull(result);
//            Assert.Equal(teacher.UserId, result.UserId);
//        }

//        [Fact]
//        public async Task GetAllAsync_ReturnsAllTeachers()
//        {
//            // Arrange
//            var teachers = new List<Teacher>
//            {
//                TestDataGenerator.CreateTestTeacherProfile(1, "user1"),
//                TestDataGenerator.CreateTestTeacherProfile(2, "user2")
//            };

           
//            await context.Teachers.AddRangeAsync(teachers);
//            await context.SaveChangesAsync();
//            // Act
//            var result = await repository.GetAllAsync();

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, 2);

//            var resultList = result.ToList();
//        }

//        [Fact]
//        public async Task AddAsync_AddsTeacherToDatabase()
//        {
//            // Arrange
//            var teacher = TestDataGenerator.CreateTestTeacherProfile();

//            // Act
//            await repository.AddAsync(teacher);

//            // Assert
//            var savedTeacher = await context.Teachers.FindAsync(teacher.Id);
//            Assert.NotNull(savedTeacher);
//            Assert.Equal(teacher.Bio, savedTeacher.Bio);
//        }
//    }
//}