using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.LeaveType
{
    public sealed record LeavesDto(Guid LeaveId, string LeaveName, int LeaveDays);
}
