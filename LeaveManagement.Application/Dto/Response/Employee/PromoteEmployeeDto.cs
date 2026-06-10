
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Dto.Response.Employee
{
    public record PromoteEmployeeDto(Guid employeeId, Role role);
}
