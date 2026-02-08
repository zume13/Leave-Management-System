using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveById
{
    internal sealed class GetLeaveByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetLeaveByIdQuery, LeavesDto>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<LeavesDto>> Handle(GetLeaveByIdQuery query, CancellationToken cancellationToken)
        {
            var leave = await _context.LeaveTypes
                .AsNoTracking()
                .Where(t => t.Id == query.LeaveId)
                .Select(t => new LeavesDto(t.Id, t.LeaveName.Value, t.Days.Days))
                .FirstOrDefaultAsync(cancellationToken);

            if (leave == null)
                return ApplicationErrors.LeaveType.LeaveTypeNotFound(query.LeaveId);

            return ResultT<LeavesDto>.Success(leave);
        }
    }
}
