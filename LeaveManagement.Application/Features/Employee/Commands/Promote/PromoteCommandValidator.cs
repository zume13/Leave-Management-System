using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.Promote
{
    internal class PromoteCommandValidator : AbstractValidator<PromoteCommand>
    {
        public PromoteCommandValidator()
        {
            RuleFor(x => x.employeeId)
                .NotEmpty().WithMessage("Employee id is required")
                .NotNull().WithMessage("Employee id is required")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Employee id must be a valid guid");

            RuleFor(x => x.role)
                .NotEmpty().WithMessage("Role is required")
                .NotNull().WithMessage("Role is required");
        }
    }
}
