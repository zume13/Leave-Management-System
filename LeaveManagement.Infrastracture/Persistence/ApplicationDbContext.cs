using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.SharedKernel.DomainEvents;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext(
        DbContextOptions op, 
        IDomainEventDispatcher dispatcher) 
        : IdentityDbContext<User>(op), IApplicationDbContext
    {
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<LeaveAllocation> LeaveAllocations => Set<LeaveAllocation>();
        public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {

            int result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEvents();

            return result;
        }

        private async Task PublishDomainEvents()
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot>()
                .Select(entry => entry.Entity)
                .SelectMany(aggregate =>
                {
                    List<IDomainEvent> domainEvents = aggregate.domainEvents;

                    aggregate.ClearDomainEvents();

                    return domainEvents;
                }).ToList();

            await dispatcher.DispatchAsync(domainEvents);
        }
    }
}
