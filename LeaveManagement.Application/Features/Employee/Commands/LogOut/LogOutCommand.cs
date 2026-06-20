using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.Employee.Commands.LogOut
{
    public record LogOutCommand(Guid employeeId) : ICommand;
}
