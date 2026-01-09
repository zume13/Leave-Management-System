using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave
{
    public record BulkLeaveAllocationDto(bool IsSuccess, string Message, List<Guid> FailedEmployeeIds);
}
