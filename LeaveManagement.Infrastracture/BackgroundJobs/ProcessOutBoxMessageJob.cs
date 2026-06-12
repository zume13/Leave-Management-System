using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Infrastructure.Persistence;
using LeaveManagement.Infrastructure.Persistence.Outbox;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProcessOutBoxMessageJob> _logger;

        public ProcessOutBoxMessageJob(
            ApplicationDbContext context,
            IDomainEventDispatcher dispatcher,
            IDomainEventTypeRegistry registry,
            IOutBoxMessageSerializer serializer,
            ILogger<ProcessOutBoxMessageJob> logger)
        {
            _context = context;
            _dispatcher = dispatcher;
            _registry = registry;
            _serializer = serializer;
            _logger = logger;
        }
        //Dispatching domain events using background jobs
        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _context.OutBoxMessages
                            .Where(o => o.ProcessedOn == null && o.retryCount < 5)
                            .Take(20)
                            .ToListAsync(context.CancellationToken);

            foreach (var message in messages)
            {

                try
                {
                    var eventType = _registry.Resolve(message.EventName);

                    if (eventType.isFailure)
                    {
                        _logger.LogError("Failed to resolve domain event type for event name {eventName}. Error: {errorMessage}", message.EventName, eventType.Error);
                        continue;
                    }

                    var domainEvent = (IDomainEvent)_serializer.Deserialize(message.Content, eventType.Value);

                    await _dispatcher.DispatchAsync([domainEvent]);

                    message.ProcessedOn = DateTime.UtcNow;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while dispatching domain events. Error: {errorMessage}", ex.Message);

                    message.Error = ($"An error occurred while dispatching domain events. Error: {ex.Message}");

                    message.retryCount++;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}