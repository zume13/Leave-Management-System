using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public sealed record LogInCommand(string Email, string Password) : ICommand<LogInDto>;
}
