using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared;


namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandHandler(IApplicationDbContext context, UserManager<User> userManager) : ICommandHandler<UpdateEmployeeCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;
        public async Task<ResultT<Guid>> Handle(UpdateEmployeeCommand command, CancellationToken token = default)
        {
            var employee = await _context.Employees.FindAsync(command.EmployeeId, token);

            if (employee == null) 
                return ApplicationErrors.Employee.EmployeeNotFound;

            await using var transaction = _context.Database.BeginTransaction();

            try
            {
                var updateResult = employee.Update(command.EmployeeName, command.Email);

                if (updateResult.isFailure)
                    return ResultT<Guid>.Failure(updateResult.Error);

                var user = await _userManager.FindByIdAsync(employee.UserId);

                if (user == null)
                    return ApplicationErrors.Employee.EmployeeNotFound;

                if (!string.IsNullOrWhiteSpace(command.Email) && user.Email != command.Email)
                {
                    var emailResult = await _userManager.SetEmailAsync(user, command.Email);

                    if (!emailResult.Succeeded)
                        return ResultT<Guid>.Failure(new Error("UserEmail.Failed", emailResult.Errors.Select(e => e.Description).ToString()!));
                }

                if (!string.IsNullOrWhiteSpace(command.EmployeeName) && user.EmployeeName != command.EmployeeName)
                    user.EmployeeName = command.EmployeeName;

                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);

                return ResultT<Guid>.Success(employee.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                return ApplicationErrors.General.InternalError;
            }
        }
    }
}
