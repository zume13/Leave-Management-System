using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    internal sealed class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
    {
        public CreateLeaveRequestCommandValidator()
        {
            RuleFor(c => c.userId)
                .NotEmpty();
            RuleFor(c => c.startDate)
                .Must(date => date.Date > DateTime.Today)
                .NotEmpty();
            RuleFor(c => c.endDate)
                .Must((request, endDate) => 
                    endDate.Date > DateTime.Today && 
                    endDate.Date > request.startDate);
            RuleFor(c => c.description)
                .MinimumLength(5);
            RuleFor(c => c.employeeId)
                .NotEmpty();
            RuleFor(c => c.leaveTypeId)
                .NotEmpty();
        }
    }
}
