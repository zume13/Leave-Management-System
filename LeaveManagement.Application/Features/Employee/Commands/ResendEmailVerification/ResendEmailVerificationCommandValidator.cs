
using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification
{
    internal class ResendEmailVerificationCommandValidator : AbstractValidator<ResendEmailVerificationCommand>
    {
        ResendEmailVerificationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
