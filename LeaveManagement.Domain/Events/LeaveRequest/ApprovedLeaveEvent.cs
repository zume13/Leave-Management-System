using LeaveManagement.SharedKernel.DomainEvents;
using SharedKernel.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Domain.Events.LeaveRequest
{
    public record ApprovedLeaveEvent() : IDomainEvent
    {
        public string EventName => DomainEventName.LeaveApproved;
    }
}
