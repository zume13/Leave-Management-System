using LeaveManagement.SharedKernel.DomainEvents;

namespace LeaveManagement.Application.Abstractions.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default);
    }
}
