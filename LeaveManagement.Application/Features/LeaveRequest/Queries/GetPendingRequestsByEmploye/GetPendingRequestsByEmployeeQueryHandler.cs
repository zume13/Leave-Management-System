using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee
{
    internal sealed class GetPendingRequestsByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetPendingRequestsByEmployeeQuery, List<GetPendingRequestsByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetPendingRequestsByEmployeeDto>>> Handle(GetPendingRequestsByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetPendingRequestsByEmployeeDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        r.StartDate,
                        r.EndDate,
                        r.Description ?? "No provided description",
                        r.LeaveDays.Days))
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetPendingRequestsByEmployeeDto>>.Success(requests);
        }
    }
}
