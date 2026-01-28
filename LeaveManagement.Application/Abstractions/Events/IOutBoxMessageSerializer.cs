using LeaveManagement.SharedKernel.DomainEvents;

namespace LeaveManagement.Application.Abstractions.Events
{
    public interface IOutBoxMessageSerializer
    {
        object Deserialize(string content, Type type);
        string Serialize(IDomainEvent domainEvent);
    }
}
