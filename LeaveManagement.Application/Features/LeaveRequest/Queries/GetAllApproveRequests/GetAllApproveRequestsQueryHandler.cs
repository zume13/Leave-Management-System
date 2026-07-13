using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;
using LeaveManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests
{
    public sealed class GetAllApproveRequestsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllApproveRequestsQuery, List<GetAllRequestsDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetAllRequestsDto>>> Handle(GetAllApproveRequestsQuery query, CancellationToken cancellationToken = default)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int pageNumber = Math.Max(1, query.pageNumber);    

            var requests = await _context.LeaveRequests
                .AsNoTracking()
                .Where(r => r.Status == LeaveRequestStatus.Approved)
                .OrderBy(r => r.RequestDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Join(_context.Employees.AsNoTracking(), 
                    r => r.EmployeeId,
                    e => e.Id,
                    (r, e) => new { Employee = e, Request = r })
                .Join(_context.LeaveTypes
                .AsNoTracking(),
                n => n.Request.LeaveTypeId,
                l => l.Id,
                (n, l) => new GetAllRequestsDto(
                    n.Request.Id,
                    n.Employee.Id,
                    l.LeaveName.Value,
                    n.Employee.Name.Value,
                    n.Request.RequestDate,
                    n.Request.ProcessedDate,
                    n.Request.StartDate,
                    n.Request.EndDate,
                    n.Request.LeaveDays.Days,
                    n.Request.Status.ToString(),
                    n.Request.Description,
                    n.Request.RejectionReason,
                    n.Request.ProcessedBy.ToString()
                ))
                .ToListAsync(cancellationToken);
                

            if (requests.Count == 0)
                return ApplicationErrors.LeaveRequests.NoRequestsFound;

            return ResultT<List<GetAllRequestsDto>>.Success(requests);
        }
    }
}
