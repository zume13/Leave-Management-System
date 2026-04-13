using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasKey(lt => lt.Id);

            builder.OwnsOne(lt => lt.LeaveName, lt =>
            {
                lt.Property(l => l.Value)
                    .HasColumnName("leave_name")
                    .IsRequired()
                    .HasMaxLength(Name.MaxLength);
            });

            builder.OwnsOne(lt => lt.Days, lt =>
            {
                lt.Property(d => d.Days)
                    .HasColumnName("days")
                    .IsRequired();
            });
        }
    }
}
