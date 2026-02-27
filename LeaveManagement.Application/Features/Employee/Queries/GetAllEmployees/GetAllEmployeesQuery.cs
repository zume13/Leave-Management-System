using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Queries.ListEmployees
{
    public sealed record GetAllEmployeesQuery(int pageNumber, int pageSize) : IQuery<List<EmployeeDto>>;
}
