using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandHandler(IApplicationDbContext context, UserManager<User> userManager) : ICommandHandler<UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<Result> Handle(UpdateEmployeeCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FindAsync(command.EmployeeId, token);

            if (employee == null) 
                return ApplicationErrors.Employee.EmployeeNotFound(command.EmployeeId);

            await using var transaction = _context.Database.BeginTransaction();

            try
            {
                var updateResult = employee.Update(command.EmployeeName, command.Email);

                if (updateResult.isFailure)
                    return Result.Failure(updateResult.Error);

                var user = await _userManager.FindByIdAsync(employee.UserId);

                if (user == null)
                    return ApplicationErrors.Employee.EmployeeNotFound(command.EmployeeId);

                if (!string.IsNullOrWhiteSpace(command.Email) && user.Email != command.Email)
                {
                    var emailResult = await _userManager.SetEmailAsync(user, command.Email);

                    if (!emailResult.Succeeded)
                        return Result.Failure(Error.Failure("UserEmail.Failed", emailResult.Errors.Select(e => e.Description).ToString()!));
                }

                if (!string.IsNullOrWhiteSpace(command.EmployeeName) && user.EmployeeName != command.EmployeeName)
                    user.EmployeeName = command.EmployeeName;

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return Result.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                return ApplicationErrors.General.InternalError;
            }
        }
    }
}
