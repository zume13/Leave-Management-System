using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveAllocation;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Response.Allocation;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetActiveLeaveAllocations;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetExpiredLeaveAllocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveAllocation
{
    [Authorize]
    [Route("LeaveManagement/Allocation")]
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [ApiController]
    public class AllocationQueryController(AllocationQueryHandlers queryHandlers) : ControllerBase
    {
        [HttpGet("Active")]
        public async Task<IActionResult> GetAllActive([FromQuery] GetActiveLeaveAllocationsQuery query)
        {
            ResultT<List<LeaveAllocationDto>> result = await queryHandlers.GetActive.Handle(query);

            return result.Match<List<LeaveAllocationDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{employeeId:guid}/All")]
        public async Task<IActionResult> GetAllAllocationsByEmployee([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetAllocationByEmployeeDto>> result = await queryHandlers.GetEmployeeAllocations.Handle(new GetAllAllocationsByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetAllocationByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Expired")]
        public async Task<IActionResult> GetExpiredAllocations([FromQuery] GetExpiredLeaveAllocationsQuery query)
        {
            ResultT<List<LeaveAllocationDto>> result = await queryHandlers.GetExpired.Handle(query);

            return result.Match<List<LeaveAllocationDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
