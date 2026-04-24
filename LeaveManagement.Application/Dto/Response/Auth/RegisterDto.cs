
namespace LeaveManagement.Application.Dto.Response.Auth
{
    public class RegisterDto 
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false;
    }
}
