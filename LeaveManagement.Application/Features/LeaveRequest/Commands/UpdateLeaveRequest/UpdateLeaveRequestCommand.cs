using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public sealed record UpdateLeaveRequestCommand(Guid LeaveRequestId, DateTime newStartDate, DateTime newEndDate, string? newDescription) : ICommand;
}
