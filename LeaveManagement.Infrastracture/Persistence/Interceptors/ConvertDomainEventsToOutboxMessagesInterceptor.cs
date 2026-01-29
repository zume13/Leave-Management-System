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
        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken ct = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
                return base.SavedChangesAsync(eventData, result, ct);

            var outBoxMessages = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregate => {

                    List<IDomainEvent> domainEvents = aggregate.domainEvents;

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

            return base.SavedChangesAsync(eventData, result, ct);
        }
    }
}
