

using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    public record UpdateEmployeeCommand(Guid EmployeeId, string? EmployeeName, string? Email) : ICommand<Guid>;
}
