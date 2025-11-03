using LMS.Data.Entities;

namespace LMS.Logic.Helpers
{
    /// JWT tokenlar bilan ishlash interfeysi
    public interface IJwtHelper
    {
        /// User uchun JWT token yaratish
        string GenerateToken(User user);

        /// Token dan user ID sini olish
        string GetUserIdFromToken(string token);
    }
}
