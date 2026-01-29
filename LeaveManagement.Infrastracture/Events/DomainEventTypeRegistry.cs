using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Domain.Events.Employees;
using LeaveManagement.Domain.Events.LeaveRequest;
using SharedKernel.DomainEvents;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Infrastructure.Events
{
    public class DomainEventTypeRegistry : IDomainEventTypeRegistry
    {
        private readonly Dictionary<string, Type> _map = new()
        {
            [DomainEventName.MemberRegistered] = typeof(MemberRegisteredEvent),
            [DomainEventName.LeaveApproved] = typeof(ApprovedLeaveEvent)
        };
        public ResultT<Type> Resolve(string eventName)
        {
            if (_map.TryGetValue(eventName, out var type))
                return ResultT<Type>.Success(type);

            return ResultT<Type>.Failure(new Error("Type.Unregistered", "Type is invalid"));
        }
    }
}
