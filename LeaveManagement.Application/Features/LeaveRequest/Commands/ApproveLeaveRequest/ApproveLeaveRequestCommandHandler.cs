using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;


namespace LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest
{
    public class ApproveLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<ApproveLeaveRequestCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(ApproveLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Requests.Where(r => r.Id == command.LeaveRequestId))
                .SingleOrDefaultAsync(e => 
                    e.Requests.Any(r => r.Id == command.LeaveRequestId), token);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound;

            var result = employee.ApproveLeaveRequest(command.LeaveRequestId, command.AdminName);

            if (result.isFailure)
                return result.Error;

            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(command.LeaveRequestId);
        }
    }
}
