using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee
{
    public sealed record GetAllAllocationsByEmployeeQuery : IQuery<List<GetAllocationByEmployeeDto>>
    {
        [FromRoute(Name = "id")]
        public Guid EmployeeId { get; init; }
        [FromQuery]
        public int pageNumber { get; init; }
        [FromQuery]
        public int pageSize { get; init; }
    }
}
