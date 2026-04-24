using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Application.Dto.Client
{
    public class ClientLogInDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
