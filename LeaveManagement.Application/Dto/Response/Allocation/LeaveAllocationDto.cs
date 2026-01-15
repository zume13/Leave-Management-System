using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.Allocation
{
    public record LeaveAllocationDto(Guid AllocationId, string EmployeeName, string LeaveName, int LeaveBalance, int YearOfValidity);
}
