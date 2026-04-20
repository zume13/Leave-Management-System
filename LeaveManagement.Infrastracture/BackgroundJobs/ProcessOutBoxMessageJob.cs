using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Infrastructure.Persistence;
using LeaveManagement.Infrastructure.Persistence.Outbox;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace LeaveManagement.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutBoxMessageJob : IJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly IDomainEventTypeRegistry _registry;
        private readonly IOutBoxMessageSerializer _serializer;

        public ProcessOutBoxMessageJob(
            ApplicationDbContext context,
            IDomainEventDispatcher dispatcher,
            IDomainEventTypeRegistry registry,
            IOutBoxMessageSerializer serializer)
        {
            _context = context;
            _dispatcher = dispatcher;
            _registry = registry;
            _serializer = serializer;
        }
        //Dispatching domain events using background jobs
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _context.OutBoxMessages
                            .Where(o => o.ProcessedOn == null)
                            .Take(20)
                            .ToListAsync(context.CancellationToken);

            List<IDomainEvent> domainEvents = [];

            foreach (var message in messages)
            {
                var eventType = _registry.Resolve(message.EventName);

                if (eventType.isFailure)
                {
                    //log the error
                    continue;
                }

                var domainEvent = _serializer.Deserialize(message.Content, eventType.Value);

                domainEvents.Add((IDomainEvent)domainEvent);
            }

            await _dispatcher.DispatchAsync(domainEvents);
            
            foreach(var message in messages)
            {
                message.ProcessedOn = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
