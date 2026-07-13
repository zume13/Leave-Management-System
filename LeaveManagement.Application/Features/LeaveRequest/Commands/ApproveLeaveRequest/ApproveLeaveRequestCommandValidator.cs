using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest
{
    internal sealed class ApproveLeaveRequestCommandValidator : AbstractValidator<ApproveLeaveRequestCommand>
    {
        public ApproveLeaveRequestCommandValidator()
        {
            RuleFor(c => c.leaveRequestId)
                .NotEmpty();
            RuleFor(c => c.approverId)
                .NotNull();
            RuleFor(c => c.employeeId)
                .NotNull();
        }
    }
}
