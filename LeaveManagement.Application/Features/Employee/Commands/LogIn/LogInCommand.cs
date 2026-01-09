using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public record LogInCommand(string Email, string Password) : ICommand<LogInDto>;
}
