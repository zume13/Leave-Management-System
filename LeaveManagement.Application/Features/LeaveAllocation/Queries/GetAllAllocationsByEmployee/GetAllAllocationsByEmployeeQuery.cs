using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee
{
    public sealed record GetAllAllocationsByEmployeeQuery(Guid employeeId, int pageSize, int pageNumber) : IQuery<List<GetAllocationByEmployeeDto>>;
}
