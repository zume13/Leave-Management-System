
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee
{
    public sealed record GetPendingRequestsByEmployeeDto(
        Guid RequestId,
        string Requestor,
        DateTime RequestDate,
        DateTime StartDate,
        DateTime EndDate,
        string Description,
        int LeaveDuration);
}
