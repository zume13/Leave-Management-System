
namespace LeaveManagement.Application.Dto.Request
{
    public record RejectLeaveRequestDto(Guid employeeId, Guid LeaveRequestId, string Reason);
}
