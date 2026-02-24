using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification
{
    public class ResendEmailVerificationCommandHandler(
        IApplicationDbContext _context,
        UserManager<User> _userManager,
        IEmailService _emailService) 
        : ICommandHandler<ResendEmailVerificationCommand, VerifyEmailDto>    
    {
        public async Task<ResultT<VerifyEmailDto>> Handle(ResendEmailVerificationCommand command, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Email.EmailInvalid);

            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user is null)
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.User.UserNotFound);

            if (user.isEmailVerified)
                return ResultT<VerifyEmailDto>.Failure(ApplicationErrors.Employee.AlreadyVerified);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id, token);

            if (employee is null)
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.User.UserNotFound);

            var newToken = Guid.NewGuid().ToString();

            var updateResult = employee.UpdateVerificationToken(newToken);

            if (updateResult.isFailure)
                return ResultT<VerifyEmailDto>.Failure(updateResult.Error);

            user.verificationToken = newToken;
            user.tokenExpiration = DateTime.UtcNow.AddDays(1);

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(token);

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync(token);
                await _emailService.SendEmailVerificationAsync(employee.Name.Value, employee.Email.Value, newToken, token);

                await transaction.CommitAsync(token);
            }
            catch (Exception)
            {
                return ResultT<VerifyEmailDto>.Failure(InfrastractureErrors.General.InternalError);
            }

            return ResultT<VerifyEmailDto>.Success(new VerifyEmailDto(
                true,
                "Verification email resent. Please check your inbox."
            ));
        }
    }
}
