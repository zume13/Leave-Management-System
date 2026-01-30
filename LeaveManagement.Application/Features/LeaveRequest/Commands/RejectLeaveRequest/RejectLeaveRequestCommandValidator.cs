using FluentValidation;


namespace LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest
{
    internal sealed class RejectLeaveRequestCommandValidator : AbstractValidator<RejectLeaveRequestCommand>
    {
        public RejectLeaveRequestCommandValidator()
        {
            RuleFor(c => c.LeaveRequestId)
                .NotEmpty();
            RuleFor(c => c.AdminName)
                .NotEmpty();
            RuleFor(c => c.Reason)
                .MinimumLength(6)
                .NotEmpty();   
        }
    }
}
