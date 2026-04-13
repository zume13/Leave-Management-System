using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RequestDate)
                .HasColumnName("request_date")
                .IsRequired();  
                
            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .HasColumnName("end_date")
                .IsRequired();  

            builder.OwnsOne<LeaveDuration>(r => r.LeaveDays, ld =>
            {
                ld.Property(d => d.Days)
                .HasColumnName("leave_days")
                    .IsRequired();
            });

            builder.Property(r => r.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(r => r.RejectionReason)
                .HasColumnName("rejection_reason")
                .HasMaxLength(500);

            builder.Property(r => r.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(r => r.ProcessedBy)
                .HasColumnName("processed_by")
                .HasMaxLength(50);
            
            builder.HasOne<Employee>()
                .WithMany(e => e.Requests)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne<LeaveType>()
                .WithMany()
                .HasForeignKey(r => r.LeaveTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
