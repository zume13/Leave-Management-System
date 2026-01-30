using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave
{
    internal sealed class RemoveLeaveTypeCommandValidator : AbstractValidator<RemoveLeaveTypeCommand>
    {
        public RemoveLeaveTypeCommandValidator()
        {
            RuleFor(c => c.LeaveTypeId)
                .NotEmpty();
        }
    }
}
