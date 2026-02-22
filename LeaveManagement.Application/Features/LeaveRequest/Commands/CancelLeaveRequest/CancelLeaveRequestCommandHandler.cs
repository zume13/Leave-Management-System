using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public sealed class CancelLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<CancelLeaveRequestCommand>
    {
        private readonly IApplicationDbContext _context = context;  
        public async Task<Result> Handle(CancelLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Requests.Where(r => r.Id == command.LeaveRequestId))
                .SingleOrDefaultAsync(e => 
                    e.Requests.Any(e => e.Id == command.LeaveRequestId), token);

            if (employee is null)
                return ApplicationErrors.LeaveRequests.RequestNotFound(command.LeaveRequestId);

            var result = employee.CancelLeaveRequest(command.LeaveRequestId);

            if (result.isFailure)
                return result.Error;

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
