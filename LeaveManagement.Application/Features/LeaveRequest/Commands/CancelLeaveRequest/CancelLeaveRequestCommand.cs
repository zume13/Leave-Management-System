using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public record CancelLeaveRequestCommand(Guid LeaveRequestId) : ICommand<bool>;
}
