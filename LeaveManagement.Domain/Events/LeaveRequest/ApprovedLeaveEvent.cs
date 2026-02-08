using LeaveManagement.SharedKernel.DomainEvents;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Events.LeaveRequest
{
    public record ApprovedLeaveEvent(string EmployeeName, string EmployeeEmail, string? Admin) : IDomainEvent
    {
        public string EventName => DomainEventName.LeaveApproved;
    }
}
