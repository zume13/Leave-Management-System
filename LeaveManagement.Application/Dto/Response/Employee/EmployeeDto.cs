using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Dto.Response.Employee
{
    public record EmployeeDto(Guid Id, string Name, string Email, EmployeeStatus Status, string Department);
}
