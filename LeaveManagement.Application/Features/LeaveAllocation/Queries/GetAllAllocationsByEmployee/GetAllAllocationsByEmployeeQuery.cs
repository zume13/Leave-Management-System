using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee
{
    public sealed record GetAllAllocationsByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetAllocationByEmployeeDto>>;
}
