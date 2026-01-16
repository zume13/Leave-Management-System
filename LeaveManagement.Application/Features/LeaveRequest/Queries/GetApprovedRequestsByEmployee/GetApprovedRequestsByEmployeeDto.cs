
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee
{
    public sealed record GetApprovedRequestsByEmployeeDto(
        Guid RequestId,
        string Requestor,
        DateTime RequestDate,
        DateTime ProcessedDate,
        DateTime StartDate,
        DateTime EndDate,
        int LeaveDuration,
        string Description,
        string Approver);
}
