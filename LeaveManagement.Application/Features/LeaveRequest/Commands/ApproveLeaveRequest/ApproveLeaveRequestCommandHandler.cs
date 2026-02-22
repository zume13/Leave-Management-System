using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest
{
    public sealed class ApproveLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<ApproveLeaveRequestCommand>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<Result> Handle(ApproveLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Requests.Where(r => r.Id == command.LeaveRequestId))
                .Include(e => e.Allocations)
                .SingleOrDefaultAsync(e => 
                    e.Requests.Any(r => r.Id == command.LeaveRequestId), token);

            if (employee is null)
                return ApplicationErrors.LeaveRequests.RequestNotFound(command.LeaveRequestId);

            var result = employee.ApproveLeaveRequest(command.LeaveRequestId, command.AdminName);

            if (result.isFailure)
                return result.Error;

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
