
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Queries.GetDashboardData
{
    public sealed class DashboardDataQueryHandler(IApplicationDbContext _context) : IQueryHandler<DashboardDataQuery, DashboardDataDto>
    {
        public async Task<ResultT<DashboardDataDto>> Handle(DashboardDataQuery query, CancellationToken cancellationToken = default)
        {
            var employeeTotal = await _context.Employees.CountAsync(cancellationToken);

            var pendingLeaveRequests = await _context.LeaveRequests
                .Where(lr => lr.Status == Domain.Enums.LeaveRequestStatus.Pending)
                .CountAsync(cancellationToken);

            var approvedLeaveRequests = await _context.LeaveRequests
                .Where(lr => lr.Status == Domain.Enums.LeaveRequestStatus.Approved)
                .CountAsync(cancellationToken);

            var rejectedLeaveRequests = await _context.LeaveRequests
                .Where(lr => lr.Status == Domain.Enums.LeaveRequestStatus.Rejected)
                .CountAsync(cancellationToken);

            return ResultT<DashboardDataDto>.Success(new DashboardDataDto(
                totalEmployees: employeeTotal,
                pendingLeaveRequests: pendingLeaveRequests,
                approvedLeaveRequests: approvedLeaveRequests,
                rejectedLeaveRequests: rejectedLeaveRequests
            ));
        }
    }
}

