using FluentValidation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave
{
    internal sealed class BulkLeaveAllocationCommandValidator : AbstractValidator<BulkLeaveAllocationCommand>
    {
        public BulkLeaveAllocationCommandValidator()
        {
            RuleFor(c => c.LeaveTypeId)
                .NotEmpty();
        }
    }
}
