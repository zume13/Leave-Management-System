using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

namespace LeaveManagement.API.Handlers.LeaveAllocation
{
    public class AllocationCommandHandlers
    {
        public ICommandHandler<AllocateLeaveCommand, Guid> Allocate { get; }
        public ICommandHandler<BulkLeaveAllocationCommand, BulkLeaveAllocationDto> BulkAllocate { get; }
        public ICommandHandler<DeleteLeaveAllocationCommand, bool> Delete { get; }
        public AllocationCommandHandlers(
            ICommandHandler<DeleteLeaveAllocationCommand, bool> _Delete,
            ICommandHandler<BulkLeaveAllocationCommand, BulkLeaveAllocationDto> _BulkAllocate,
            ICommandHandler<AllocateLeaveCommand, Guid> _Allocate)
        {
            Allocate = _Allocate;
            BulkAllocate = _BulkAllocate;
            Delete = _Delete;   
        }
    }
}
