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
        : ICommandHandler<ResendEmailVerificationCommand, VerifyEmailDto>    
    {
        public async Task<ResultT<VerifyEmailDto>> Handle(ResendEmailVerificationCommand command, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Email.EmailInvalid);

            var employee = await _context.Employees
                .FindAsync(command.Email, token);

            if (employee is null)
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.User.UserNotFound);

            var newToken = EmailVerificationToken.Create(DateTime.UtcNow.AddHours(24), employee.Id);

            if(newToken.isFailure)
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Email.EmailVerificationTokenCreationFailed);

            var updateResult = employee.UpdateVerificationToken(newToken.Value.Id.ToString());

            if (updateResult.isFailure)
                return ResultT<VerifyEmailDto>.Failure(updateResult.Error);

                await _context.SaveChangesAsync(token);
                await _emailService.SendEmailVerificationAsync(employee.Name.Value, employee.Email.Value, newToken.Value.Id.ToString(), token);


            return ResultT<VerifyEmailDto>.Success(new VerifyEmailDto(
                true,
                "Verification email resent. Please check your inbox."
            ));
        }
    }
}
