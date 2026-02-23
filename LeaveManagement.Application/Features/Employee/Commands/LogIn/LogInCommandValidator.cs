using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    internal sealed class LogInCommandValidator : AbstractValidator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .NotEmpty().WithMessage("Email should not be empty");
            RuleFor(c => c.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .NotEmpty().WithMessage("Password should not be empty");
        }
    }
}
