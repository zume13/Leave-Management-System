
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetActiveLeaveAllocations
{
    internal sealed class GetActiveLeaveAllocationsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetActiveLeaveAllocationsQuery, List<LeaveAllocationDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<LeaveAllocationDto>>> Handle(GetActiveLeaveAllocationsQuery query, CancellationToken cancellationToken)
        {
            var currentYear = DateTime.UtcNow.Year;
            var allocations = await _context.LeaveAllocations
                .AsNoTracking()
                .Where(a => a.RemainingDays > 0 && a.Year == currentYear)
                .Join(_context.Employees.AsNoTracking(),
                    a => a.EmployeeId,
                    e => e.Id,
                    (a, e) => new LeaveAllocationDto(
                    a.Id, 
                    e.Name.Value,
                    a.LeaveName,
                    a.RemainingDays,
                    a.Year))
                .ToListAsync(cancellationToken);

            if (allocations.Count == 0)
                return ApplicationErrors.LeaveAllocation.NoAllocationsFound;

            return ResultT<List<LeaveAllocationDto>>.Success(allocations);
        }
    }
}
