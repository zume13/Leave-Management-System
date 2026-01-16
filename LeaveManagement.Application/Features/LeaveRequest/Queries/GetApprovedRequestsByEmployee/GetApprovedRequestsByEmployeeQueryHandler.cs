using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee
{
    internal sealed class GetApprovedRequestsByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetApprovedRequestsByEmployeeQuery, List<GetApprovedRequestsByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetApprovedRequestsByEmployeeDto>>> Handle(GetApprovedRequestsByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetApprovedRequestsByEmployeeDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        (DateTime)r.ProcessedDate!,
                        r.StartDate,
                        r.EndDate,
                        r.LeaveDays.Days,
                        r.Description ?? "No provided description",
                        r.ProcessedBy!))
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetApprovedRequestsByEmployeeDto>>.Success(requests);
        }
    }
}
