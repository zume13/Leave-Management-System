using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee
{
    public sealed record GetAllRequestByEmployeeDto(
        Guid RequestId,
        string Requestor,
        DateTime RequestDate,
        DateTime StartDate,
        DateTime EndDate,
        int LeaveDuration,
        string Description,
        LeaveRequestStatus Status);
}
