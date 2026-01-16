using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveById
{
    public sealed record GetLeaveByIdQuery(Guid LeaveId) : IQuery<LeavesDto>;
}
