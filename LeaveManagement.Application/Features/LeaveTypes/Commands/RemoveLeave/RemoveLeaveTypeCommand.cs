using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave
{
    public sealed record RemoveLeaveTypeCommand(Guid LeaveTypeId) : ICommand<bool>;
}
