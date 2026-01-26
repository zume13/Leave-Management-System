using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Infrastructure.Persistence.Outbox
{
    public sealed class OutBoxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime OccuredOn { get; set; } 
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; } 

    }
}
