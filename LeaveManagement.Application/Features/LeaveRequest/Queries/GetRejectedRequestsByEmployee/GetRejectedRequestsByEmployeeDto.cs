using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee
{
    public sealed record GetRejectedRequestsByEmployeeDto(
        Guid RequestId,
        string Requestor,
        DateTime RequestDate,
        DateTime ProcessedDate,
        DateTime StartDate,
        DateTime EndDate,
        int LeaveDuration,
        string Reason,
        string ProcessedBy);
}
