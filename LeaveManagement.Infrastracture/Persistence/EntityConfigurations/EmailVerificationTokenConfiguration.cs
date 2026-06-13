using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne<Employee>()
                .WithOne()
                .HasForeignKey<EmailVerificationToken>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.Property(e => e.ExpiryDate)
                .HasColumnName("expiry_date")
                .IsRequired();

            builder.Property(e => e.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            builder.Property(e => e.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(e => e.UsedAt)
                .HasColumnName("used_at");
        }
    }
}
