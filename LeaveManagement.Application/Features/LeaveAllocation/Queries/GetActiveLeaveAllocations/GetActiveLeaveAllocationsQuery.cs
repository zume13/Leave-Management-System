using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetActiveLeaveAllocations
{
    public sealed record GetActiveLeaveAllocationsQuery(int pageSize, int pageNumber) : IQuery<List<LeaveAllocationDto>>;
}

