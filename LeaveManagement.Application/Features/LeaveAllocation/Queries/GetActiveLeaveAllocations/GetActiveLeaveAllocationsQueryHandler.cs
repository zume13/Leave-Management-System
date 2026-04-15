using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.Allocation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetActiveLeaveAllocations
{
    public sealed class GetActiveLeaveAllocationsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetActiveLeaveAllocationsQuery, List<LeaveAllocationDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<LeaveAllocationDto>>> Handle(GetActiveLeaveAllocationsQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int pageNumber = Math.Max(1, query.pageNumber);

            var currentYear = DateTime.UtcNow.Year;
            var allocations = await _context.LeaveAllocations
                .AsNoTracking()
                .Where(a => !a.IsExpired)
                .OrderBy(e => e.CreationDate)
                .Skip((pageNumber - 1 ) * pageSize)
                .Take(pageSize)
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
