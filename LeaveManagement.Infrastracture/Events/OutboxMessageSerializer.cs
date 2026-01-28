
using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.SharedKernel.DomainEvents;
using System.Text.Json;

namespace LeaveManagement.Infrastructure.Events
{
    public class OutboxMessageSerializer(JsonSerializerOptions _options) : IOutBoxMessageSerializer
    {
        public object Deserialize(string content, Type type) => (IDomainEvent)JsonSerializer.Deserialize(content, type, _options)!;

        public string Serialize(IDomainEvent domainEvent) => JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), _options);
    }
}
