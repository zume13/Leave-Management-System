using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Departments { get; }
        DbSet<Employee> Employees { get; }
        DbSet<LeaveAllocation> LeaveAllocations { get; }
        DbSet<LeaveType> LeaveTypes { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
