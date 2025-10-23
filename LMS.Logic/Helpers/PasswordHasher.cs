using LMS.Data.Entities;

namespace LMS.Logic.Helpers
{
    /// Parollarni hash qilish va tekshirish interfeysi
    /// Bu Identity ning o'rnini bosadi
    public interface IPasswordHasher
    {
        /// Parolni hash qilish
        string HashPassword(string password);
        

        /// Hash qilingan parolni tekshirish
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }

    /// BCrypt algoritmi bilan parol hash qilish
    /// Bu eng xavfsiz usullardan biri
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // BCrypt avtomatik salt generate qiladi
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // BCrypt hash ni avtomatik tekshiradi
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}