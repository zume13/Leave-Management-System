using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Auth;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public sealed record LogInCommand(string Email, string Password) : ICommand<LogInDto>;
}
