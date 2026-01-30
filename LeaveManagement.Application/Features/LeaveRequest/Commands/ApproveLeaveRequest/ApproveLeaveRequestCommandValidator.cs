using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest
{
    internal sealed class ApproveLeaveRequestCommandValidator : AbstractValidator<ApproveLeaveRequestCommand>
    {
        public ApproveLeaveRequestCommandValidator()
        {
            RuleFor(c => c.LeaveRequestId)
                .NotEmpty();
            RuleFor(c => c.AdminName)
                .NotEmpty();
        }
    }
}
