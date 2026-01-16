using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetExpiredLeaveAllocations
{
    public sealed record GetExpiredLeaveAllocationsQuery : IQuery<List<LeaveAllocationDto>>;
}
