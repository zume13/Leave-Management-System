using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LeaveManagement.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Departments { get; }
        DbSet<Employee> Employees { get; }
        DbSet<LeaveAllocation> LeaveAllocations { get; }
        DbSet<LeaveType> LeaveTypes { get; }
        DbSet<LeaveRequest> LeaveRequests { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}
