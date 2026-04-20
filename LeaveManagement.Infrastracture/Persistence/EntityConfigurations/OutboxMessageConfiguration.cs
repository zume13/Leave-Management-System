
using LeaveManagement.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutBoxMessage>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.EventName)
                .HasColumnName("event_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Type)
                .HasColumnName("type")  
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Content)
                .HasColumnName("content")
                .IsRequired();

            builder.Property(o => o.OccuredOn)
                .HasColumnName("occured_on")
                .IsRequired();

            builder.Property(o => o.ProcessedOn)
                .HasColumnName("processed_on");

            builder.Property(o => o.Error)
                .HasColumnName("error")
                .HasMaxLength(500);
        }
    }
}
