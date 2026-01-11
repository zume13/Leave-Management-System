using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public sealed record CancelLeaveRequestCommand(Guid LeaveRequestId) : ICommand<bool>;
}
