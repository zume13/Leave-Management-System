
namespace LeaveManagement.Infrastructure.Persistence.Outbox
{
    public sealed class OutBoxMessage
    {
        public Guid Id { get; set; }
        public string EventName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime OccuredOn { get; set; } 
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; } 

    }
}
