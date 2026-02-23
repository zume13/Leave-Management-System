using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    internal sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(c => c.EmployeeId)
                .NotEmpty().WithMessage("Employee id should not be empty");
            RuleFor(c => c.EmployeeName)
                .MinimumLength(5).WithMessage("Employee name should not be shorter than 5 characters");
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("Please enter a valid Email Address");
        }
    }
}
