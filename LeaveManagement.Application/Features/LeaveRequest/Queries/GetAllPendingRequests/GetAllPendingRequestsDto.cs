
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests
{
    public sealed record GetAllPendingRequestsDto(
        Guid RequestId,
        Guid RequestorId,
        string Requestor,
        DateTime RequestDate,
        DateTime StartDate,
        DateTime EndDate,
        string Description,
        int LeaveDuration);
}
