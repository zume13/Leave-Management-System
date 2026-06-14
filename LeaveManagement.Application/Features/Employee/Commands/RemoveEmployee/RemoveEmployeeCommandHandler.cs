using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee
{
    public sealed class RemoveEmployeeCommandHandler(IApplicationDbContext context) : ICommandHandler<RemoveEmployeeCommand>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result> Handle(RemoveEmployeeCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == command.employeeId, token);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound(command.employeeId);

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
