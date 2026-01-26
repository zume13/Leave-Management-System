using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee
{
    internal sealed class GetRejectedRequestsByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetRejectedRequestsByEmployeeQuery, List<GetRejectedRequestsByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetRejectedRequestsByEmployeeDto>>> Handle(GetRejectedRequestsByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetRejectedRequestsByEmployeeDto(
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

            return ResultT<List<GetRejectedRequestsByEmployeeDto>>.Success(requests);
        }
    }
}
