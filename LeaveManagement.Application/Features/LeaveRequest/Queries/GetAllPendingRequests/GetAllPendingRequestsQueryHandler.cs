using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests
{
    internal sealed class GetAllPendingRequestsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllPendingRequestsQuery, List<GetAllPendingRequestsDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllPendingRequestsDto>>> Handle(GetAllPendingRequestsQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.IsPending() == true)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetAllPendingRequestsDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        r.StartDate,
                        r.EndDate,
                        r.Description ?? "No description provided",
                        r.LeaveDays.Days))
                    .ToListAsync(cancellationToken);

            if (requests.Count() == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllPendingRequestsDto>>.Success(requests);
        }
    }
}
