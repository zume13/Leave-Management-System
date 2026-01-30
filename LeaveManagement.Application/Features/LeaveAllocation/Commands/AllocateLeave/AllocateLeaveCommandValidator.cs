using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave
{
    internal sealed class AllocateLeaveCommandValidator : AbstractValidator<AllocateLeaveCommand>
    {
        public AllocateLeaveCommandValidator()
        {
            RuleFor(c => c.EmployeeId)
                .NotEmpty();
            RuleFor(c => c.LeaveTypeId)
                .NotEmpty();
        }
    }
}
