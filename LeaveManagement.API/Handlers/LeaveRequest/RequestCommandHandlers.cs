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
        public ICommandHandler<ApproveLeaveRequestCommand> Approve { get; }
        public ICommandHandler<CancelLeaveRequestCommand> Cancel { get; }
        public ICommandHandler<CreateLeaveRequestCommand, Guid> Create { get; }
        public ICommandHandler<RejectLeaveRequestCommand> Reject { get; }
        public ICommandHandler<UpdateLeaveRequestCommand> Update { get; }
        public RequestCommandHandlers(
            ICommandHandler<ApproveLeaveRequestCommand> approve,
            ICommandHandler<CancelLeaveRequestCommand> cancel,
            ICommandHandler<CreateLeaveRequestCommand, Guid> create,
            ICommandHandler<RejectLeaveRequestCommand> reject,
            ICommandHandler<UpdateLeaveRequestCommand> update)
        {
            Approve = approve;
            Cancel = cancel;
            Create = create;
            Reject = reject;
            Update = update;
        }
    }
}
