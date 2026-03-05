using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.Employee.Commands.AssignRole
{
    public record AssignRoleCommand(string userId, string role) : ICommand;
}
