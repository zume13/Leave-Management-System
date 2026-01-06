using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public class LogInDto
    {
        public bool IsSuccessful { get; set; }
        public string UserId { get; set; } = null!;
        public string Accesstoken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
