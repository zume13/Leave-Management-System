using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.AuthResponse
{
    public class UserDto
    {
        Name EmployeeName { get; set; } = null!;
        Email Email { get; set; } = null!;
    }
}
