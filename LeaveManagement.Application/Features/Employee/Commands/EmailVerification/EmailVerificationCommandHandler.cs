using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    public class EmailVerificationCommandHandler(
        IApplicationDbContext _context) 
        : ICommandHandler<EmailVerificationCommand>
    {
        public async Task<Result> Handle(EmailVerificationCommand command, CancellationToken token = default)
        {        
            if (string.IsNullOrWhiteSpace(command.token))
                return Result.Failure(ApplicationErrors.Employee.InvalidToken);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.VerificationToken == command.token, token);

            var Etoken = await _context.EmailVerificationTokens.FindAsync(Guid.Parse(command.token));

            if (employee is null)
                return Result.Failure(ApplicationErrors.Employee.InvalidToken);

            if (Etoken is null || !Etoken.IsValid || Etoken.Id.ToString() != employee.VerificationToken)
                return Result.Failure(ApplicationErrors.Employee.InvalidToken);

            if(Etoken.IsExpired)
                return Result.Failure(ApplicationErrors.Employee.ExpiredToken(Etoken.Id.ToString()));

            var verifyResult = employee.VerifyEmail();

            if (verifyResult.isFailure)
                return Result.Failure(verifyResult.Error);

            await _context.SaveChangesAsync(token);

            return Result.Success();
        }
    }
}
