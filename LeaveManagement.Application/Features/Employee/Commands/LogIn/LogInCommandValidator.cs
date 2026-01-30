using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    internal sealed class LogInCommandValidator : AbstractValidator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress()
                .NotEmpty();
            RuleFor(c => c.Password)
                .MinimumLength(8)
                .NotEmpty();
        }
    }
}
