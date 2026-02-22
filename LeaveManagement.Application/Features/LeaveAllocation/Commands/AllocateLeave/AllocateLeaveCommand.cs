using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave
{
    public record AllocateLeaveCommand(Guid EmployeeId, Guid LeaveTypeId) : ICommand<Guid>;
}
