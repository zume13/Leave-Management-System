using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public sealed record GetEmployeesByDepartmentQuery(Guid DeptId) : IQuery<List<GetEmployeesByDepartmentDto>>;
}
