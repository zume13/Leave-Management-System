using LeaveManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Departments { get; }
        DbSet<Employee> Employees { get; }
        DbSet<LeaveAllocation> LeaveAllocations { get; }
        DbSet<Employee> LeaveTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
