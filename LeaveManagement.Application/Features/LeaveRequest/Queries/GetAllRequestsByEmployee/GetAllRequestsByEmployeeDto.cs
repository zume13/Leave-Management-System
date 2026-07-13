
namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee
{
    public sealed record GetAllRequestsByEmployeeDto(
        Guid Id,
        string LeaveName,
        DateTime RequestDate,
        DateTime? ProcessedDate,
        DateTime StartDate,
        DateTime EndDate,
        int LeaveDays,
        string Status,
        string? Description,
        string? RejectionReason,
        string? ProcessedBy
    );

}
