
namespace LeaveManagement.Infrastructure.DateTimeProvider
{
    public static class DateExpiry
    {
        public static DateTime accessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
        public static DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
    }
}
