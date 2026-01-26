using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests
{
    internal sealed class GetAllRejectedRequestsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRejectedRequestsDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllRejectedRequestsDto>>> Handle(GetAllRejectedRequestsQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.IsRejected() == true)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetAllRejectedRequestsDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        (DateTime)r.ProcessedDate!,
                        r.StartDate,
                        r.EndDate,
                        r.LeaveDays.Days,
                        r.RejectionReason ?? "No provided reason",
                        r.ProcessedBy!))
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllRejectedRequestsDto>>.Success(requests);
        }
    }
}
