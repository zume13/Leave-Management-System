using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<Result> Handle(UpdateEmployeeCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FindAsync(command.EmployeeId, token);

            if (employee is null) 
                return ApplicationErrors.Employee.EmployeeNotFound(command.EmployeeId);

                var updateResult = employee.Update(command.EmployeeName, command.Email);

                if (updateResult.isFailure)
                    return Result.Failure(updateResult.Error);

                await _context.SaveChangesAsync(token);

                return Result.Success();
        }
    }
}
