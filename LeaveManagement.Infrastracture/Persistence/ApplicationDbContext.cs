using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Infrastracture.Persistence
{
    public class ApplicationDbContext(DbContextOptions op) : IdentityDbContext<User>(op), IApplicationDbContext
    {
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<LeaveAllocation> LeaveAllocations => Set<LeaveAllocation>();
        public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
