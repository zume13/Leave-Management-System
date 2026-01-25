using LeaveManagement.SharedKernel.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.DomainEvents
{
    public interface IDomainEventHandler<in T>
      where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken ct = default);
    }
}
