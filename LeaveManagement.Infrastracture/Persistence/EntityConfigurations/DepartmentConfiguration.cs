
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Infrastructure.Persistence.EntityConfigurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.OwnsOne(d => d.DepartmentName, deptName =>
            {
                deptName.Property(deptname => deptname.Value)
                .HasColumnName("department_name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
            });
        }
    }
}
