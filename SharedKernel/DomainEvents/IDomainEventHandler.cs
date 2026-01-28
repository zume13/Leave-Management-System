using LeaveManagement.SharedKernel.DomainEvents;

namespace SharedKernel.DomainEvents
{
    public interface IDomainEventHandler<in T>
      where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken ct = default);
    }
}
