using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave
{
    public sealed record UpdateLeaveTypeCommand(Guid LeaveTypeId, string NewName, int NewDays) : ICommand;
}
