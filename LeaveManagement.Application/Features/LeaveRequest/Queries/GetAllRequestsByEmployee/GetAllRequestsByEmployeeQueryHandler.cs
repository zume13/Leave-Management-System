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
            int pageSize = query.pageSize <= 0
                ? NumericConstant.DefaultPageSize
                : NumericConstant.MaxPageSize(query.pageSize);

            int pageNumber = Math.Max(1, query.pageNumber);

            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .OrderByDescending(r => r.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Join(_context.LeaveTypes.AsNoTracking(),
                    re => re.LeaveTypeId,
                    lt => lt.Id,
                    (re, lt) => new GetAllRequestsByEmployeeDto(
                        re.Id,
                        lt.LeaveName.Value,
                        re.RequestDate,
                        re.ProcessedDate,
                        re.StartDate,
                        re.EndDate,
                        re.LeaveDays.Days,
                        re.Status.ToString(),
                        re.Description,
                        re.RejectionReason,
                        re.ProcessedBy.ToString()
                       )).ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            

            return ResultT<List<GetAllRequestsByEmployeeDto>>.Success(requests);
        }
    }
}
