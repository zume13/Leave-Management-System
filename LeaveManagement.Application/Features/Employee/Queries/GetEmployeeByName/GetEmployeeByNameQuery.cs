using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeeByName
{
    public sealed record GetEmployeeByNameQuery(
    string Name,
    int PageNumber,
    int PageSize
) : IQuery<List<EmployeeDto>>;
}
