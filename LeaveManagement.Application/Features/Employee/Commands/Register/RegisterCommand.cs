using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public record RegisterCommand(string Email, string EmployeeName, string Password, Guid DepartmentId): ICommand<RegisterDto>;
}
