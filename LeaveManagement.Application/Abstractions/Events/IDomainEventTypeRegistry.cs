
namespace LeaveManagement.Application.Abstractions.Events
{
    public interface IDomainEventTypeRegistry
    {
        Type Resolve(string eventName);
    }
}
