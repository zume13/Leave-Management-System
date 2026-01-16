using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee
{
    public sealed record GetRejectedRequestsByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetRejectedRequestsByEmployeeDto>>;
}
