using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress()
                .NotEmpty();
            RuleFor(c => c.EmployeeName)
                .NotEmpty()
                .MinimumLength(5);
            RuleFor(c => c.Password)
                .MinimumLength(8)
                .NotEmpty();
            RuleFor(c => c.DepartmentId)
                .NotEmpty();
        }
    }
}
