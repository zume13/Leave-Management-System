using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification
{
    public class ResendEmailVerificationCommandHandler(
        IApplicationDbContext _context,
        IEmailService _emailService) 
        : ICommandHandler<ResendEmailVerificationCommand>    
    {
        public async Task<Result> Handle(ResendEmailVerificationCommand command, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                return Result.Failure(ApplicationErrors.Email.EmailInvalid);

            var employee = await _context.Employees
                .FindAsync(command.Email, token);


            if (employee is null)
                return Result.Failure(InfrastractureErrors.User.UserNotFound);

            var oldToken = await _context.EmailVerificationTokens
                                    .FindAsync(employee.Id, token);

            if(oldToken is null)
                return Result.Failure(ApplicationErrors.Email.InvalidEmailVerificationToken);

            oldToken.Revoke();

            var newToken = EmailVerificationToken.Create(employee.Id);

            if(newToken.isFailure)
                return Result.Failure(ApplicationErrors.Email.EmailVerificationTokenCreationFailed);

            var updateResult = employee.UpdateVerificationToken(newToken.Value.Id.ToString());

            if (updateResult.isFailure)
                return Result.Failure(updateResult.Error);

                await _context.SaveChangesAsync(token);
                await _emailService.SendEmailVerificationAsync(employee.Name.Value, employee.Email.Value, newToken.Value.Id.ToString(), token);

            return Result.Success();
        }
    }
}
