using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DomainEvents;
using System.Collections.Concurrent;


namespace LeaveManagement.Infrastructure.Events
{
    public class DomainEventsDispatcher(IServiceProvider sp) : IDomainEventDispatcher
    {
        private readonly IServiceProvider _sp = sp;
        private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
        private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();
        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                using IServiceScope scope = _sp.CreateScope();

                var domainEventType = domainEvent.GetType();
                var handlerType = HandlerTypeDictionary.GetOrAdd(domainEventType,
                    et => typeof(IDomainEventHandler<>).MakeGenericType(domainEventType));

                IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

                foreach(object? handler in handlers)
                {
                    if(handler is null)
                        continue;

                    var handlerWrapper = HandlerWrapper.Create(handler, handlerType);

                    await handlerWrapper.Handle(domainEvent, ct);
                }
            }
        }

        private abstract class HandlerWrapper
        {
            public abstract Task Handle(IDomainEvent domainEvent, CancellationToken ct);

            public static HandlerWrapper Create(object handler, Type domainEventType)
            {
                Type wrapperType = WrapperTypeDictionary.GetOrAdd(domainEventType, et => typeof(HandlerWrapper<>).MakeGenericType(domainEventType));

                return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
            }
        }

        private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
        {
            private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;
            public override async Task Handle(IDomainEvent domainEvent, CancellationToken ct)
            {
                await _handler.Handle((T)domainEvent, ct);
            }
        }

    }
}
