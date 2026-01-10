using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;
using System.Data.Entity;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<CancelLeaveRequestCommand, bool>
    {
        private readonly IApplicationDbContext _context = context;  
        public async Task<ResultT<bool>> Handle(CancelLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Requests.Where(r => r.Id == command.LeaveRequestId))
                .SingleOrDefaultAsync(e => 
                    e.Requests.Any(e => e.Id == command.LeaveRequestId), token);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound;

            var result = employee.CancelLeaveRequest(command.LeaveRequestId);

            if (result.isFailure)
                return result.Error;

            await _context.SaveChangesAsync(token);

            return ResultT<bool>.Success(true);
        }
    }
}
