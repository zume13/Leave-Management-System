using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Auth
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
