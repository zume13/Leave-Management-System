using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeave
{
    internal sealed class GetAllLeaveQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllLeavesQuery, List<LeavesDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<LeavesDto>>> Handle(GetAllLeavesQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int pageNumber = Math.Max(1, query.pageNumber);

            var leaves = await _context.LeaveTypes
                .AsNoTracking()
                .OrderBy(l => l.LeaveName.Value)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new LeavesDto(t.Id, t.LeaveName.Value, t.Days.Days))
                .ToListAsync(cancellationToken);

            if (leaves.Count == 0)
                return ApplicationErrors.LeaveType.NoLeaveFound;

            return ResultT<List<LeavesDto>>.Success(leaves);
        }
    }
}
