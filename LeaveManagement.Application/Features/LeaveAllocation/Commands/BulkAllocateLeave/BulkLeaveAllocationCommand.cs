

using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveAllocation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave
{
    public sealed record BulkLeaveAllocationCommand(Guid LeaveTypeId) : ICommand<BulkLeaveAllocationDto>;
}
