using LeaveManagement.SharedKernel.DomainEvents;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Events.Employees
{
    public record MemberRegisteredEvent(Guid employeeId) : IDomainEvent
    {
        public string EventName => DomainEventName.MemberRegistered;
    }
}
