namespace LMS.Shared.Dtos.StudentDtos;

public class UpdateStudentDto
{
    /// Yangi student ID (masalan: "STU2025001U")
    public string StudentId { get; set; }

    /// O'quvchi qachon ro'yxatdan o'tgan
    /// Agar kerak bo'lsa, enrollment date ni o'zgartirish mumkin
    public DateTime? EnrollmentDate { get; set; }
}