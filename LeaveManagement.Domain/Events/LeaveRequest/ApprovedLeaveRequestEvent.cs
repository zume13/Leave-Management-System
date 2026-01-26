using LeaveManagement.SharedKernel.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Domain.Events.LeaveRequest
{
    public record ApprovedLeaveRequestEvent() : IDomainEvent;
}
