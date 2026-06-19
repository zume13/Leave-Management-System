
using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification
{
    internal class ResendEmailVerificationCommandValidator : AbstractValidator<ResendEmailVerificationCommand>
    {
       public ResendEmailVerificationCommandValidator()
        {
            RuleFor(x => x.token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
