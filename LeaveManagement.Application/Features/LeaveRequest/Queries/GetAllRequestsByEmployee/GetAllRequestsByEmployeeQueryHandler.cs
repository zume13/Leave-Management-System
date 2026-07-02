using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee
{
    internal sealed class GetRequestsByEmployeeQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetAllRequestsByEmployeeQuery, List<GetAllRequestsByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<ResultT<List<GetAllRequestsByEmployeeDto>>> Handle(
            GetAllRequestsByEmployeeQuery query,
            CancellationToken cancellationToken)
        {
            int pageSize = query.PageSize <= 0
                ? NumericConstant.DefaultPageSize
                : NumericConstant.MaxPageSize(query.PageSize);

            int pageNumber = Math.Max(1, query.PageNumber);

            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .OrderByDescending(r => r.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Join(
                    _context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetAllRequestsByEmployeeDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        r.ProcessedDate,
                        r.StartDate,
                        r.EndDate,
                        r.LeaveDays.Days,
                        r.Status.ToString(),
                        r.Description,
                        r.RejectionReason,
                        r.ProcessedBy.ToString()
                    ))
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllRequestsByEmployeeDto>>.Success(requests);
        }
    }
}
