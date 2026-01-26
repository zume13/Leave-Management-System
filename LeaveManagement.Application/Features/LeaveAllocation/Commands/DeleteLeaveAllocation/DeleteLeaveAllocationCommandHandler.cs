using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public sealed class DeleteLeaveAllocationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteLeaveAllocationCommand, bool>
    {
        private readonly IApplicationDbContext _context = context;  
        public async Task<ResultT<bool>> Handle(DeleteLeaveAllocationCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees
                .Include(e => e.Allocations.Where(a => a.Id == command.AllocationId))
                .SingleOrDefaultAsync(e => 
                    e.Allocations.Any(a => a.Id == command.AllocationId), token);

            if(employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound;

            var result = employee.RemoveAllocation(command.AllocationId);

            if (result.isFailure)
                return ResultT<bool>.Failure(result.Error);

            await _context.SaveChangesAsync(token);

            return ResultT<bool>.Success(true);
        }
    }
}
