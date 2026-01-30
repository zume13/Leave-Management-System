using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    internal sealed class DeleteLeaveAllocationCommandValidator : AbstractValidator<DeleteLeaveAllocationCommand>
    {
        public DeleteLeaveAllocationCommandValidator()
        {
            RuleFor(c => c.AllocationId)
                .NotEmpty();
        }
    }
}
