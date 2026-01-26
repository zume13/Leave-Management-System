using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave
{
    public sealed class AllocateLeaveCommandHandler(IApplicationDbContext context) : ICommandHandler<AllocateLeaveCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(AllocateLeaveCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FindAsync(command.EmployeeId, token);

            if(employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound;

            var leaveType = await _context.LeaveTypes.FindAsync(command.LeaveTypeId, token);
            
            if(leaveType is null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound;

            var allocateResult = employee.AllocateLeave(leaveType);

            if(allocateResult.isFailure)
                return ResultT<Guid>.Failure(allocateResult.Error);

            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(allocateResult.Value);
        }
    }
}
