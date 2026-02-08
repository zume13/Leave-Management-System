using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public sealed class UpdateLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateLeaveRequestCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(UpdateLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => 
                    e.Requests.Where(r => r.Id == command.LeaveRequestId))
                .SingleOrDefaultAsync(e => 
                    e.Requests.Any(r => r.Id == command.LeaveRequestId), token);

            if (employee is null)
                return ApplicationErrors.LeaveRequests.RequestNotFound(command.LeaveRequestId);

            var editResult = employee.EditRequest(command.LeaveRequestId, command.newStartDate, command.newEndDate, command.newDescription);

            if (editResult.isFailure)
                return editResult.Error;

            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(command.LeaveRequestId);
        }
    }
}
