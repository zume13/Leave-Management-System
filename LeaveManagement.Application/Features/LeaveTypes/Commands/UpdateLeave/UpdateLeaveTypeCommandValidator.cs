using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave
{
    internal sealed class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
    {
        public UpdateLeaveTypeCommandValidator()
        {
            RuleFor(c => c.NewName)
                .MinimumLength(6)
                .NotEmpty();
            RuleFor(c => c.NewDays)
                .NotEmpty();
        }
    }
}
