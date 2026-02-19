using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public record GetEmployeesByDepartmentDto(Guid EmployeeId, string EmployeeName, string Email, EmployeeStatus status);
    
}
