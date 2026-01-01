using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Application.Features.Employee.Commands.CreateEmployee
{
    public record CreateEmployeeCommand(Name Name, Email Email, Guid DepartmentId) : ICommand<CreateEmployeeDto>;
    
}
