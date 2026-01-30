using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave
{
    internal sealed class CreateLeaveCommandValidator : AbstractValidator<CreateLeaveCommand>
    {
        public CreateLeaveCommandValidator()
        {
            RuleFor(c => c.DefaultDays)
                .NotEmpty();
            RuleFor(c => c.Name)
                .MinimumLength(5)
                .NotEmpty();
        }
    }
}
