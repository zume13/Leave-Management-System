using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee
{
    internal sealed class GetAllRequestByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllRequestByEmployeeQuery, List<GetAllRequestByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllRequestByEmployeeDto>>> Handle(GetAllRequestByEmployeeQuery query, CancellationToken cancellationToken)
        {
            var request = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.EmployeeId)
                .Join(_context.Employees.AsNoTracking(),
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new GetAllRequestByEmployeeDto(
                        r.Id,
                        e.Name.Value,
                        r.RequestDate,
                        r.StartDate,
                        r.EndDate,
                        r.LeaveDays.Days,
                        r.Description ?? "No description provided",
                        r.Status))
                .ToListAsync(cancellationToken);

            if (request.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllRequestByEmployeeDto>>.Success(request);  
        }
    }
}
