using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Application.Models
{
    public class User : IdentityUser
    {
        public Name EmployeeName { get; set; } = null!;
    }
}
