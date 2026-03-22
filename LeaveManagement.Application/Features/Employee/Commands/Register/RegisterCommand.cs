using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Auth;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public sealed record RegisterCommand(string Email, string EmployeeName, string Password, Guid DepartmentId): ICommand<RegisterDto>;
}
