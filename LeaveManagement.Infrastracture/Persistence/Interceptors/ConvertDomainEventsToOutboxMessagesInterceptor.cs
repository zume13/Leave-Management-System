using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Infrastructure.Persistence.Outbox;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LeaveManagement.Infrastructure.Persistence.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor(IOutBoxMessageSerializer _serializer) : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken ct = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
                return base.SavingChangesAsync(eventData, result, ct);

            var outBoxMessages = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregate => {

                    var domainEvents = aggregate.domainEvents.ToList();

                    aggregate.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutBoxMessage
                    {
                        Id = Guid.NewGuid(),
                        EventName = domainEvent.EventName,
                        Type = domainEvent.GetType().Name,
                        Content = _serializer.Serialize(domainEvent),
                        OccuredOn = DateTime.UtcNow,
                    })
                .ToList();

            dbContext.Set<OutBoxMessage>().AddRange(outBoxMessages);

            return base.SavingChangesAsync(eventData, result, ct);
        }
    }
}
