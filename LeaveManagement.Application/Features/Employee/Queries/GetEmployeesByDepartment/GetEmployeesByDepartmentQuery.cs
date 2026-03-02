using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public sealed record GetEmployeesByDepartmentQuery(Guid deptId, int pageSize, int pageNumber) : IQuery<List<GetEmployeesByDepartmentDto>>;
}
