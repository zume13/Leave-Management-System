using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType
{
    public record UpdateLeaveTypeCommand(Guid LeaveTypeId, string NewName, int NewDays) : ICommand<Guid>;
}
