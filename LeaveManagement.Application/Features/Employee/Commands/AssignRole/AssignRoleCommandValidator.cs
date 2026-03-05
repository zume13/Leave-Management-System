
using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.AssignRole
{
    internal class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("User id is required")
                .NotNull().WithMessage("User id is required")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("User id must be a valid guid");

            RuleFor(x => x.role)
                .NotEmpty().WithMessage("Role is required")
                .NotNull().WithMessage("Role is required");
        }
    }
}
