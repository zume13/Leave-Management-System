using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

namespace LeaveManagement.API.Handlers.LeaveRequest
{
    public class RequestCommandHandlers
    {
        public ICommandHandler<ApproveLeaveRequestCommand, Guid> Approve { get; }
        public ICommandHandler<CancelLeaveRequestCommand, bool> Cancel { get; }
        public ICommandHandler<CreateLeaveRequestCommand, Guid> Create { get; }
        public ICommandHandler<RejectLeaveRequestCommand, Guid> Reject { get; }
        public ICommandHandler<UpdateLeaveRequestCommand, Guid> Update { get; }
        public RequestCommandHandlers(
            ICommandHandler<ApproveLeaveRequestCommand, Guid> approve,
            ICommandHandler<CancelLeaveRequestCommand, bool> cancel,
            ICommandHandler<CreateLeaveRequestCommand, Guid> create,
            ICommandHandler<RejectLeaveRequestCommand, Guid> reject,
            ICommandHandler<UpdateLeaveRequestCommand, Guid> update)
        {
            Approve = approve;
            Cancel = cancel;
            Create = create;
            Reject = reject;
            Update = update;
        }
    }
}
