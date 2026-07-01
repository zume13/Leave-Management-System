
namespace LeaveManagement.Application.Features.Employee.Queries.GetDashboardData
{
    public record DashboardDataDto(
        int totalEmployees,
        int pendingLeaveRequests,
        int approvedLeaveRequests,
        int rejectedLeaveRequests
    );
}
