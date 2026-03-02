using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee
{
    internal sealed class GetAllRequestByEmployeeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllRequestByEmployeeQuery, List<GetAllRequestByEmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllRequestByEmployeeDto>>> Handle(GetAllRequestByEmployeeQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int pageNumber = Math.Max(1, query.pageNumber);

            var request = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.EmployeeId == query.employeeId)
                .OrderBy(r => r.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
