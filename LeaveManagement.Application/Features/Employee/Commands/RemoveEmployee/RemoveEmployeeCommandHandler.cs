using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee
{
    public sealed class RemoveEmployeeCommandHandler(IApplicationDbContext context, UserManager<User> userManager) : ICommandHandler<RemoveEmployeeCommand>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<Result> Handle(RemoveEmployeeCommand command, CancellationToken token = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(token);

            var employee = await _context.Employees.FindAsync(command.employeeId, token);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound(command.employeeId);

            var user = await _userManager.FindByIdAsync(employee.UserId.ToString());

            if (user is null)
                return ApplicationErrors.General.InternalError;

            try
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    return ApplicationErrors.General.InternalError;

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                return ApplicationErrors.General.InternalError;
            }

            return Result.Success();
        }
    }
}
