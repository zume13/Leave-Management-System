using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests
{
    public sealed record GetAllRequestsDto(
       Guid Id,
       Guid EmployeeId,
       string LeaveName,
       string EmployeeName,
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
