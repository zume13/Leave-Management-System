

using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave
{
    public record BulkLeaveAllocationCommand(Guid LeaveTypeId) : ICommand<BulkLeaveAllocationDto>;
}
