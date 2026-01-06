using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Application.Models
{
    public class User : IdentityUser
    {
        public Name EmployeeName { get; set; } = null!;
        public bool isEmailVerified { get; set; } = false;
        public string? verificationToken { get; set; } 
        public DateTime tokenExpiration { get; set; }
    }
}
