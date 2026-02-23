using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    internal class EmailVerificationCommandValidator : AbstractValidator<EmailVerificationCommand>
    {
        EmailVerificationCommandValidator()
        {
            RuleFor(x => x.token)
                .NotEmpty().WithMessage("Token is required.")
                .NotNull().WithMessage("Token cannot be null.");
        }
    }
}
