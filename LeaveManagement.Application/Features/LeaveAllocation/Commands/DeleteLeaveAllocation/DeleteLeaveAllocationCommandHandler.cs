using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteLeaveAllocationCommand, bool>
    {
        private readonly IApplicationDbContext _context = context;  
        public async Task<ResultT<bool>> Handle(DeleteLeaveAllocationCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FindAsync(command.EmployeeId, token);

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
