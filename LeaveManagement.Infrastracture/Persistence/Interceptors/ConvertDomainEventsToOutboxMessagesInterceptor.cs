using LeaveManagement.Domain.Primitives;
using LeaveManagement.Infrastructure.Persistence.Outbox;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace LeaveManagement.Infrastructure.Persistence.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken ct = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
                return base.SavedChangesAsync(eventData, result, ct);

            dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(x => x.Entity)
                .Select(aggregate => {

                    List<IDomainEvent> domainEvents = aggregate.domainEvents.ToList();

                    aggregate.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutBoxMessage
                    {
                        Id = Guid.NewGuid(),
                        Type = domainEvent.GetType().Name,
                        Content = JsonConvert.SerializeObject(
                            domainEvent,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            })
                    })
                .ToList();

            return base.SavedChangesAsync(eventData, result, ct);
        }
    }
}
