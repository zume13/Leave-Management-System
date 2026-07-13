using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public sealed record GetEmployeesByDepartmentQuery(Guid deptId, int pageSize, int pageNumber) : IQuery<List<EmployeeDto>>;
}
