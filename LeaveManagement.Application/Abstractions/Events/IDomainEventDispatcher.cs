using LeaveManagement.Application.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Abstractions.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct);
    }
}
