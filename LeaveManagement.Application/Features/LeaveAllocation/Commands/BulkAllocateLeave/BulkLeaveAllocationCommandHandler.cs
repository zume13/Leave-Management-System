using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Application.Dto.Response.LeaveAllocation;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave
{
    public sealed class BulkLeaveAllocationCommandHandler(IApplicationDbContext context) : ICommandHandler<BulkLeaveAllocationCommand, BulkLeaveAllocationDto>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<BulkLeaveAllocationDto>> Handle(BulkLeaveAllocationCommand command, CancellationToken token = default)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(command.LeaveTypeId, token);

            if (leaveType is null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound;

            var employees = await _context.Employees.Where(e => e.Status == Domain.Enums.EmployeeStatus.Active).ToListAsync(token);

            if (employees.Count == 0)
                return ApplicationErrors.Employee.NoEmployeesFound;

            var failedEmployees = new List<Guid>();

            await using var transaction = await _context.Database.BeginTransactionAsync(token);

            try
            {
                foreach (var employee in employees)
                {
                    var allocationResult = employee.AllocateLeave(leaveType);
                    if (allocationResult.isFailure)
                        failedEmployees.Add(employee.Id);
                }

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return ResultT<BulkLeaveAllocationDto>.Success(
                    new BulkLeaveAllocationDto
                    (
                        IsSuccess: failedEmployees.Count == 0,
                        Message: $"Leave allocated to {employees.Count - failedEmployees.Count} employees successfully.",
                        FailedEmployeeIds: failedEmployees
                    )
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                return ResultT<BulkLeaveAllocationDto>.Failure(new Error("BulkLeaveAllocationFailed", $"Bulk leave allocation failed: {ex.Message}"));
            }
        }
    }
}
