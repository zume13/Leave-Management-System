using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeaveType
{
    public sealed class RemoveLeaveTypeCommandHandler(IApplicationDbContext context) : ICommandHandler<RemoveLeaveTypeCommand, bool>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<bool>> Handle(RemoveLeaveTypeCommand command, CancellationToken token = default)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(command.LeaveTypeId);

            if (leaveType is null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound;

            _context.LeaveTypes.Remove(leaveType);

            await _context.SaveChangesAsync(token);

            return ResultT<bool>.Success(true);
        }
    }
}
