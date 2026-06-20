
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.LogOut
{
    public class LogOutCommandHandler(IApplicationDbContext _context) : ICommandHandler<LogOutCommand>
    {
        public async Task<Result> Handle(LogOutCommand command, CancellationToken token = default)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.EmployeeId == command.employeeId, token);

            if (refreshToken is null)
                return ApplicationErrors.Employee.InvalidToken;

            var result = refreshToken.Revoke();

            if (result.isFailure)
                return result.Error;

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
