using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Auth
{
    public class LogInDto
    {
        Email Email { get; set; } = null!;
        string Password { get; set; } = null!;
    }
}
