using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.Promote
{
    public sealed class PromoteCommandHandler(IApplicationDbContext _context) : ICommandHandler<PromoteCommand>
    {
        public async Task<Result> Handle(PromoteCommand command, CancellationToken token)
        {
            var employee = await _context.Employees.FindAsync(command.employeeId, token);  

            if(employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound(command.employeeId);
            
            var isSuccess = employee.Promote(command.role);

            if(isSuccess.isFailure)
                return Result.Failure(isSuccess.Error);

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
