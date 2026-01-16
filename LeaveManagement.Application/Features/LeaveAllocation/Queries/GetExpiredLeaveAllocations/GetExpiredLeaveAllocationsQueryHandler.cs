using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetExpiredLeaveAllocations
{
    internal sealed class GetExpiredLeaveAllocationsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetExpiredLeaveAllocationsQuery, List<LeaveAllocationDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<LeaveAllocationDto>>> Handle(GetExpiredLeaveAllocationsQuery query, CancellationToken cancellationToken)
        {
            var allocations = await _context.LeaveAllocations
                .AsNoTracking()
                .Where(a => a.IsExpired == true)
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

            if (allocations.Count() == 0)
                return ApplicationErrors.LeaveAllocation.NoAllocationsFound;

            return ResultT<List<LeaveAllocationDto>>.Success(allocations);
        }
    }
}
