using LeaveManagement.Application.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Domain.Events.Employees
{
    public record MemberRegisteredEvent() : IDomainEvent;
}
