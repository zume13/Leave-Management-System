
namespace LeaveManagement.Application.Dto.Response.Auth
{
    public class RegisterDto 
    {
        public string Email { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false;
    }
}
