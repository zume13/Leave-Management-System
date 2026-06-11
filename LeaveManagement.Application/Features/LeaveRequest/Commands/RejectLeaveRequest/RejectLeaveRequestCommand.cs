using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest
{
    public sealed record RejectLeaveRequestCommand(Guid employeeId, Guid LeaveRequestId, string Reason, Guid processorId) : ICommand;
}
