using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    internal sealed class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
    {
        public UpdateLeaveRequestCommandValidator()
        {
            RuleFor(c => c.LeaveRequestId)
                .NotEmpty();
            RuleFor(c => c.newStartDate)
                .Must(date => date.Date > DateTime.Today)
                .NotEmpty();
            RuleFor(c => c.newEndDate)
                .Must((request, endDate) => endDate.Date > DateTime.Today && endDate.Date > request.newStartDate)
                .NotEmpty();
            RuleFor(c => c.newDescription)
                .MinimumLength(6);
        }
    }
}
