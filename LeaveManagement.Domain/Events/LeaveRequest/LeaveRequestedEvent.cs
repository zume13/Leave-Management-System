using LeaveManagement.SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Events.LeaveRequest
{
    public record LeaveRequestedEvent() : IDomainEvent;
}
