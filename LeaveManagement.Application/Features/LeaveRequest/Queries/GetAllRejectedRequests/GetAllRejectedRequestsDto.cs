
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests
{
    public sealed record GetAllRejectedRequestsDto(
        Guid RequestId,
        string Requestor,
        DateTime RequestDate,
        DateTime ProcessedDate,
        DateTime StartDate,
        DateTime EndDate,
        int LeaveDuration,
        string Reason,
        string ProcessedBy);
}
