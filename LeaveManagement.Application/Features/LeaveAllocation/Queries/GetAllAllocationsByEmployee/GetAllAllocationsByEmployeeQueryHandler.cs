using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee
{
    internal sealed class GetAllAllocationsByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllAllocationsByEmployeeQuery, List<GetAllocationByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllocationByEmployeeDto>>> Handle(GetAllAllocationsByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var allocations = await _context.LeaveAllocations
                .AsNoTracking()
                .Where(a => a.EmployeeId == query.EmployeeId)
                .Select(a => new GetAllocationByEmployeeDto(
                    a.Id,
                    a.LeaveName,
                    a.RemainingDays,
                    a.Year))
                .ToListAsync(cancellationToken);
             
            if (allocations.Count == 0)
                return ApplicationErrors.LeaveAllocation.NoAllocationsFound;

            return ResultT<List<GetAllocationByEmployeeDto>>.Success(allocations);
        }
    }
}
