using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
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
            var leaves = await _context.LeaveTypes
                .AsNoTracking()
                .Select(t => new LeavesDto(t.Id, t.LeaveName.Value, t.Days.Days))
                .ToListAsync(cancellationToken);

            if (leaves.Count == 0)
                return ApplicationErrors.LeaveType.NoLeaveFound;

            return ResultT<List<LeavesDto>>.Success(leaves);
        }
    }
}
