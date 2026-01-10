using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public record UpdateLeaveRequestCommand(Guid LeaveRequestId, DateTime newStartDate, DateTime newEndDate, string? newDescription) : ICommand<Guid>;
}
