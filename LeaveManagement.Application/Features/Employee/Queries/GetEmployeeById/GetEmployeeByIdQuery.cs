using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployee
{
    public sealed record GetEmployeeByIdQuery(Guid employeeId) : IQuery<EmployeeDto>;
}
