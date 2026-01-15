using LeaveManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    public record GetEmployeesByDepartmentDto(Guid EmployeeId, string EmployeeName, string Email, EmployeeStatus status);
    
}
