using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Events
{
    public interface IDomainEventTypeRegistry
    {
        ResultT<Type> Resolve(string eventName);
    }
}
