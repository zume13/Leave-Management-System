
using LeaveManagement.SharedKernel.DomainEvents;

namespace LeaveManagement.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _events = new();
        public IReadOnlyCollection<IDomainEvent> domainEvents => _events;
        protected AggregateRoot(Guid id) : base(id){}
        protected AggregateRoot() { }

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _events.Clear();
        }
    }
}
