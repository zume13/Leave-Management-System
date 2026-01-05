using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public class LogInDto
    {
        string Email { get; set; } = null!;
        string Password { get; set; } = null!;
    }
}
