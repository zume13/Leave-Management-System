
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests
{
    public record GetAllApproveRequestsDto(
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
