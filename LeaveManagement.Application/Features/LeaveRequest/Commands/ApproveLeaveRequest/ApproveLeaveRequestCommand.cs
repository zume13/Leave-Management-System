using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest
{
    public sealed record ApproveLeaveRequestCommand(Guid employeeId, Guid LeaveRequestId, Guid approverId) : ICommand;
}
