﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Dtos.UserDtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string StudentId { get; set; }

        // Teacher uchun (optional)
        public string Bio { get; set; }
        [Required(ErrorMessage = "Qualifications are required")]
        public string Qualifications { get; set; }
        [Required(ErrorMessage = "Specialization is required")]
        public string? Specialization { get; set; } = null;


    }
}