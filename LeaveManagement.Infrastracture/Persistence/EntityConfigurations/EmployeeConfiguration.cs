using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.OwnsOne<Name>(e => e.Name, name =>
            {
                name.Property(n => n.Value)
                .HasColumnName("name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
            });

            builder.OwnsOne<Email>(e => e.Email, email =>
            {
                email.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();
            });

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(e => e.VerificationToken)
                .HasColumnName("token");

            builder.HasOne<Department>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.DeptId);

            builder.HasMany<LeaveAllocation>(e => e.Allocations)
                .WithOne()
                .HasForeignKey(la => la.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<LeaveRequest>(e => e.Requests)
                .WithOne()
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
