//using Microsoft.EntityFrameworkCore;
//using Moq;
//using LMS.Data;
//using LMS.Data.Entities;
//using LMS.Data.Repositories;
//using LMS.Tests.TestData;
//using Xunit;

//namespace LMS.Tests.Repositories
//{
//    public class UserRepositoryTests
//    {
//        private readonly AppDbContext context;
//        private readonly UserRepository repository;

//        public UserRepositoryTests()
//        {
//            // In-memory database
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            this.context = new AppDbContext(options);
//            this.repository = new UserRepository(context);
//        }

//        [Fact]
//        public async Task GetByIdAsync_UserExists_ReturnsUser()
//        {
//            // Arrange
//            var user = TestDataGenerator.CreateTestUser();
//            context.Users.Add(user);
//            await context.SaveChangesAsync();

//            // Act
//            var result = await repository.GetByIdAsync(user.Id);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(user.Id, result.Id);
//            Assert.Equal(user.Email, result.Email);
//        }

//        [Fact]
//        public async Task GetByIdAsync_UserNotExists_ReturnsNull()
//        {
//            // Act
//            var result = await repository.GetByIdAsync("nonexistent");

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task GetByEmailAsync_UserExists_ReturnsUser()
//        {
//            // Arrange
//            var user = TestDataGenerator.CreateTestUser();
//            context.Users.Add(user);
//            await context.SaveChangesAsync();

//            // Act
//            var result = await repository.GetByEmailAsync(user.Email);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(user.Email, result.Email);
//        }

//        [Fact]
//        public async Task GetAllAsync_ReturnsAllUsers()
//        {
//            // Arrange
//            var users = new List<User>
//            {
//                TestDataGenerator.CreateTestUser("1", "user1@test.com"),
//                TestDataGenerator.CreateTestUser("2", "user2@test.com")
//            };

//            context.Users.AddRange(users);
//            await context.SaveChangesAsync();

//            // Act
//            var result = await repository.GetAllAsync();

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.Count());
//        }

//        [Fact]
//        public async Task AddAsync_AddsUserToDatabase()
//        {
//            // Arrange
//            var user = TestDataGenerator.CreateTestUser();

//            // Act
//            await repository.AddAsync(user);

//            // Assert
//            var savedUser = await context.Users.FindAsync(user.Id);
//            Assert.NotNull(savedUser);
//            Assert.Equal(user.Email, savedUser.Email);
//        }

//        [Fact]
//        public async Task UpdateAsync_UpdatesUserInDatabase()
//        {
//            // Arrange
//            var user = TestDataGenerator.CreateTestUser();
//            context.Users.Add(user);
//            await context.SaveChangesAsync();

//            // Act
//            user.FirstName = "UpdatedName";
//            await repository.UpdateAsync(user);

//            // Assert
//            var updatedUser = await context.Users.FindAsync(user.Id);
//            Assert.NotNull(updatedUser);
//            Assert.Equal("UpdatedName", updatedUser.FirstName);
//        }

//        [Fact]
//        public async Task DeleteAsync_RemovesUserFromDatabase()
//        {
//            // Arrange
//            var user = TestDataGenerator.CreateTestUser();
//            context.Users.Add(user);
//            await context.SaveChangesAsync();

//            // Act
//            await repository.DeleteAsync(user.Id);

//            // Assert
//            var deletedUser = await context.Users.FindAsync(user.Id);
//            Assert.Null(deletedUser);
//        }
//    }
//}