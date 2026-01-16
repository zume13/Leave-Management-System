using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests
{
    internal sealed class GetAllApproveRequestsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllApproveRequestsQuery, List<GetAllApproveRequestsDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllApproveRequestsDto>>> Handle(GetAllApproveRequestsQuery query, CancellationToken cancellationToken)
        {
            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.IsApproved() == true)
                .Join(_context.Employees.AsNoTracking(), 
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetAllApproveRequestsDto(
                    r.Id,
                    e.Name.Value,
                    r.RequestDate,
                    (DateTime)r.ProcessedDate!,
                    r.StartDate,
                    r.EndDate,
                    r.LeaveDays.Days,
                    r.Description ?? "No description provided",
                    r.ProcessedBy ?? "System"))
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllApproveRequestsDto>>.Success(requests);
        }
    }
}
