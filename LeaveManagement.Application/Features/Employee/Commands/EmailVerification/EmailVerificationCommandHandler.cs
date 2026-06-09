using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    public class EmailVerificationCommandHandler(
        IApplicationDbContext _context) 
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

            var verifyResult = employee.VerifyEmail();

            if (verifyResult.isFailure)
                return ResultT<VerifyEmailDto>.Failure(verifyResult.Error);

            await _context.SaveChangesAsync(token);

            return ResultT<VerifyEmailDto>.Success(new VerifyEmailDto(
                true,
                "Email verified successfully. You can now log in."
            ));
        }
    }
}
