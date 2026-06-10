using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Features.Employee.Commands.Promote
{
    public record PromoteCommand(Guid employeeId, Role role) : ICommand<PromoteEmployeeDto>;
}
