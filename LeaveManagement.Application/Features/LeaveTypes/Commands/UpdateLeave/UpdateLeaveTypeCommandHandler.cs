
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave
{
    public sealed class UpdateLeaveTypeCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateLeaveTypeCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(UpdateLeaveTypeCommand command, CancellationToken token = default)
        {
            var leaveType = _context.LeaveTypes.Find(command.LeaveTypeId);

            if (leaveType is null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound;

            var updateResult = leaveType.Update(command.NewName, command.NewDays);

            if (updateResult.isFailure)
                return updateResult.Error;

            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(leaveType.Id);
        }
    }
}
