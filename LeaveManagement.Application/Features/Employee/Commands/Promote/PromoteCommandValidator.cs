using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.Promote
{
    internal class PromoteCommandValidator : AbstractValidator<PromoteCommand>
    {
        public PromoteCommandValidator()
        {
            RuleFor(x => x.employeeId)
                .NotEmpty().WithMessage("Employee id is required")
                .NotNull().WithMessage("Employee id is required");

            RuleFor(x => x.role)
                .NotEmpty().WithMessage("Role is required")
                .NotNull().WithMessage("Role is required");
        }
    }
}
