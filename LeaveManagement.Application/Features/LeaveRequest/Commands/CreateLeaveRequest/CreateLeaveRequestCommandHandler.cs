using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;


namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateLeaveRequestCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(CreateLeaveRequestCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Requests)
                .SingleOrDefaultAsync(e => e.Id == command.employeeId, token);
            
            if(employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound; 

            var leaveType = await _context.LeaveTypes.FindAsync(command.leaveTypeId, token);

            if(leaveType is null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound;

            var leaveRequest = employee.RequestLeave(command.startDate, command.endDate, command.description, leaveType);

            if (leaveRequest.isFailure)
                return leaveRequest.Error;

            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(leaveRequest.Value.Id);
        }
    }
}
