using LeaveManagement.SharedKernel.DomainEvents;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Events.Employees
{
    public record MemberRegisteredEvent(string EmployeeName, string EmployeeEmail, string? VerificationToken) : IDomainEvent
    {
        public string EventName => DomainEventName.MemberRegistered;
    }
}
