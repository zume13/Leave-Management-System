
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
        var currentYear = DateTime.UtcNow.Year;
        public async Task<ResultT<List<LeaveAllocationDto>>> Handle(GetActiveLeaveAllocationsQuery query, CancellationToken cancellationToken)
        {
            var allocations = await _context.LeaveAllocations.AsNoTracking()
                .Where(a => a.RemainingDays > 0 && a.Year == currentYear)
                .Join(_context.Employees,
                    a => a.EmployeeId,
                    e => e.Id,
                    (a, e) => new { a, e })
                .Join(_context.LeaveTypes,
                    firstJoin => firstJoin.a.LeaveTypeId,
                    l => l.Id,
                    (firstJoin, l) => new { firstJoin, l })
                .Select(secondJoin => new LeaveAllocationDto(
                    secondJoin.firstJoin.a.Id, 
                    secondJoin.firstJoin.e.Name.Value, 
                    secondJoin.l.LeaveName.Value, 
                    secondJoin.firstJoin.a.RemainingDays,
                    secondJoin.firstJoin.a.Year))
                .ToListAsync(cancellationToken);

            if (allocations.Count == 0)
                return ApplicationErrors.LeaveAllocation.NoAllocationsFound;

            return ResultT<List<LeaveAllocationDto>>.Success(allocations);
        }
    }
}
