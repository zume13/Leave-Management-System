using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public sealed record GetEmployeesByDepartmentQuery : IQuery<List<GetEmployeesByDepartmentDto>>
    {
        [FromRoute(Name = "deptId")]
        public Guid DeptId { get; init; }
        [FromQuery]
        public int pageNumber { get; init; }
        [FromQuery]
        public int pageSize { get; init; }
    }
}
