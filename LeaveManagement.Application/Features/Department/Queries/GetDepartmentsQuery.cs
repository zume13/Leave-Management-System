using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Department;

namespace LeaveManagement.Application.Features.Department.Queries
{
    public record GetDepartmentsQuery(int pageSize, int pageNumber) : IQuery<List<DepartmentDto>>;
}
