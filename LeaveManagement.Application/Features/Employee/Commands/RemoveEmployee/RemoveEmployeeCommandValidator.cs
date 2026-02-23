using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee
{
    internal sealed class RemoveEmployeeCommandValidator : AbstractValidator<RemoveEmployeeCommand>
    {
        public RemoveEmployeeCommandValidator()
        {
            RuleFor(c => c.employeeId)
                .NotEmpty().WithMessage("Employee id should not be empty");
        }
    }
}
