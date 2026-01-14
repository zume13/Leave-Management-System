using LeaveManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.Employee
{
    public record EmployeeDto(Guid Id, string Name, string Email, EmployeeStatus Status, string Department);
}
