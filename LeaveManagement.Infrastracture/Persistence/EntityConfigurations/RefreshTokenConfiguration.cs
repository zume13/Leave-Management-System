
using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);    

            builder.Property(rt => rt.Token)
                .HasColumnName("token")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked")
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(rt => rt.ReplacedByToken)
                .HasColumnName("replaced_by_token")
                .HasMaxLength(512);

            builder.Property(rt => rt.UserId)
                .HasColumnName("user_id")
                .IsRequired();
        }
    }
}
