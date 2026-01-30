using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest
{
    public sealed record RejectLeaveRequestCommand(Guid LeaveRequestId, string Reason, string AdminName) : ICommand<Guid>;
}
