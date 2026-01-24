using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Application.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Infrastructure.Events
{
    public class DomainEventsDispatcher(IServiceProvider sp) : IDomainEventDispatcher
    {
        private readonly IServiceProvider _sp = sp;
        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct)
        {
            foreach (var domainEvent in domainEvents)
            {
                using IServiceScope scope = _sp.CreateScope();
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

                foreach(dynamic? handler in handlers)
                {
                    if (handler is null)
                        continue;


                }
            }
        }
    }
}
