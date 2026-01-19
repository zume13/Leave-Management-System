
namespace LeaveManagement.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        protected AggregateRoot(Guid id) : base(id){}
        protected AggregateRoot() { }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }
        protected void ClearDomainEvents()
        {
            _events.Clear();
        }
    }
}
