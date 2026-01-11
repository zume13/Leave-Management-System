using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.Employee
{
    public record EmployeeDto(Guid id, string Name, string Email, string Status, string Department);
}
