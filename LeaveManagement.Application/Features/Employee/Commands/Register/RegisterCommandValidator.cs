using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("Please enter a valid Emaild Adress")
                .NotEmpty().WithMessage("Email should not be empty");
            RuleFor(c => c.EmployeeName)
                .NotEmpty().WithMessage("Employee name should not be empty")
                .MinimumLength(5).WithMessage("Employee name should not be shorter that 5 characters");
            RuleFor(c => c.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .NotEmpty().WithMessage("Password should not be empty");
            RuleFor(c => c.DepartmentId)
                .NotEmpty().WithMessage("Department id should not be empty");
        }
    }
}
