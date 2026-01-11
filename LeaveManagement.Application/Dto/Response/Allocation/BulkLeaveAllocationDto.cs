using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.LeaveAllocation
{
    public sealed record BulkLeaveAllocationDto(bool IsSuccess, string Message, List<Guid> FailedEmployeeIds);
}
