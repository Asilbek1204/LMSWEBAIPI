//using LMS.Data.Entities;
//using LMS.Shared.Dtos.UserDtos;
//using System;

//namespace LMS.Tests.TestData
//{
//    public static class TestDataGenerator
//    {
//        public static User CreateTestUser(string id = "1", string email = "test@example.com")
//        {
//            return new User
//            {
//                Id = id,
//                UserName = email,
//                Email = email,
//                FirstName = "Test",
//                LastName = "User",
//                CreatedAt = DateTime.UtcNow
//            };
//        }

//        public static Teacher CreateTestTeacherProfile(int id = 55, string userId = "1")
//        {
//            return new Teacher
//            {
//                Id = id,
//                UserId = userId,
//                Bio = "Test Bio",
//                Qualifications = "Test Qualifications",
//                Specialization = "Test Specialization",
//                YearsOfExperience = 5
//            };
//        }

//        public static Student CreateTestStudentProfile(int id = 1, string userId = "1")
//        {
//            return new Student
//            {
//                Id = id,
//                UserId = userId,
//                StudentId = "STU001",
//                EnrollmentDate = DateTime.UtcNow
//            };
//        }

//        public static CreateUserDto CreateTestCreateUserDto()
//        {
//            return new CreateUserDto
//            {
//                Email = "newuser@example.com",
//                Password = "password123",
//                FirstName = "New",
//                LastName = "User",
//                Role = "Student"
//            };
//        }

//        public static UpdateUserDto CreateTestUpdateUserDto()
//        {
//            return new UpdateUserDto
//            {
//                FirstName = "Updated",
//                LastName = "Name",
//                Bio = "Updated Bio",
//                Qualifications = "Updated Qualifications"
//            };
//        }
//    }
//}