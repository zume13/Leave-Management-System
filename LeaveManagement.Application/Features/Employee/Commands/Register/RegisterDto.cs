using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Domain.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public class RegisterDto 
    {
        public string Email { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid DepartmentId { get; set; }
    }
}
