using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    internal class EmailVerificationCommandHandler(
        IApplicationDbContext _context,
        UserManager<User> _userManager) 
        : ICommandHandler<EmailVerificationCommand, VerifyEmailDto>
    {
        public async Task<ResultT<VerifyEmailDto>> Handle(EmailVerificationCommand command, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(command.token))
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Employee.InvalidToken);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.VerificationToken == command.token, token);

            if (employee is null)
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Employee.InvalidToken);

            var user = await _userManager.FindByIdAsync(employee.UserId);

            if (user is null)
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.User.UserNotFound);

            if (user.tokenExpiration < DateTime.UtcNow)
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.Email.TokenExpired);

            if (user.isEmailVerified)
                return ResultT<VerifyEmailDto>.Failure(DomainErrors.Employee.AlreadyVerified);

            var verifyResult = employee.VerifyEmail();

            if (verifyResult.isFailure)
                return ResultT<VerifyEmailDto>.Failure(verifyResult.Error);

            user.isEmailVerified = true;
            user.verificationToken = null;
            user.tokenExpiration = null;

            try
            {
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync(token);
            }
            catch (Exception)
            {
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.General.InternalError);
            }

            return ResultT<VerifyEmailDto>.Success(new VerifyEmailDto(
                true,
                "Email verified successfully. You can now log in."
            ));
        }
    }
}
