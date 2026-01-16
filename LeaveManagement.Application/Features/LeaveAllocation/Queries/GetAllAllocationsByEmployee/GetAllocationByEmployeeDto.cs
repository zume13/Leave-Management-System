
namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee
{
    public record GetAllocationByEmployeeDto(Guid AllocationId, string LeaveName, int LeaveBalance, int YearOfValidity);
}
