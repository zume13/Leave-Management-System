using LeaveManagement.SharedKernel.DomainEvents;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Events.LeaveRequest
{
    public record RejectedLeaveEvent(string employeeName, string employeeEmail, Guid requestId, string rejectionReason) : IDomainEvent
    {
        public string EventName => DomainEventName.LeaveRejected;
    }
}
