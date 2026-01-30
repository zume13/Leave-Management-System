
using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    internal sealed class CancelLeaveRequestCommandValidator : AbstractValidator<CancelLeaveRequestCommand>
    {
        public CancelLeaveRequestCommandValidator()
        {
            RuleFor(c => c.LeaveRequestId)
                .NotEmpty();
        }
    }
}
