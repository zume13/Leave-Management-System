
namespace LeaveManagement.Application.Dto.Response.LeaveAllocation
{
    public sealed record BulkLeaveAllocationDto(bool IsSuccess, string Message, List<Guid> FailedEmployeeIds);
}
