using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Domain.Events.Employees;
using LeaveManagement.Domain.Events.LeaveRequest;
using SharedKernel.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Infrastructure.Events
{
    public class DomainEventTypeRegistry : IDomainEventTypeRegistry
    {
        private readonly Dictionary<string, Type> _map = new()
        {
            [DomainEventName.MemberRegistered] = typeof(MemberRegisteredEvent),
            [DomainEventName.LeaveApproved] = typeof(ApprovedLeaveEvent)
        };
        public Type Resolve(string eventName)
        {
            if (_map.TryGetValue(eventName, out var type))
                return type;

            return 
        }
    }
}
