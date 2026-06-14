using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    internal class EmailVerificationCommandValidator : AbstractValidator<EmailVerificationCommand>
    {
       public EmailVerificationCommandValidator()
        {
            RuleFor(x => x.token)
                .NotNull().WithMessage("Token cannot be null.")
                .NotEmpty().WithMessage("Token cannot be empty.");
        }
    }
}
