using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest
{
    public class RejectLeaveRequestCommandHandler : ICommandHandler<RejectLeaveRequestCommand, bool>
    {
        public Task<ResultT<bool>> Handle(RejectLeaveRequestCommand command, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
