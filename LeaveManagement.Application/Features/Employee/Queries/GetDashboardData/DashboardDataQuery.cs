using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.Employee.Queries.GetDashboardData
{
    public record DashboardDataQuery() : IQuery<DashboardDataDto>;
}
