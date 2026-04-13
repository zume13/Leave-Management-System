using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class LeaveAllocationConfiguration : IEntityTypeConfiguration<LeaveAllocation>
    {
        public void Configure(EntityTypeBuilder<LeaveAllocation> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreationDate)
                .HasColumnName("creation_date")
                .IsRequired();

            builder.OwnsOne(a => a.LeaveDays, leaveDays =>
            {
                leaveDays.Property(leaveDays => leaveDays.Days)
                .HasConversion(v => v, v => LeaveDuration.Create(v).Value.Days)
                .HasColumnName("leave_days");
            });



            builder.Property(a => a.LeaveName)
                .HasColumnName("name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired();

            builder.Property(a => a.UsedDays)
                .HasColumnName("used_days");

            builder.Ignore(a => a.RemainingDays);

            builder.Property(a => a.Year)
                .HasColumnName("year_validity")
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany(e => e.Allocations)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<LeaveType>()
                .WithMany()
                .HasForeignKey(a => a.LeaveTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
