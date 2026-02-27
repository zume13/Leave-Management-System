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
using static LeaveManagement.API.Constants.Constants;

namespace LeaveManagement.API.Controllers.LeaveAllocation
{
    [Authorize]
    [Route("LeaveManagement/Allocation")]
    [EnableRateLimiting(RateLimits.PerUser)]
    [ApiController]
    public class AllocationQueryController(AllocationQueryHandlers queryHandlers) : ControllerBase
    {
        [HttpGet("Active")]
        public async Task<IActionResult> GetAllActive([FromQuery] GetActiveLeaveAllocationsQuery query)
        {
            ResultT<List<LeaveAllocationDto>> result = await queryHandlers.GetActive.Handle(query);

            return result.Match<List<LeaveAllocationDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{id:guid}/All")]
        public async Task<IActionResult> GetAllAllocationsByEmployee(GetAllAllocationsByEmployeeQuery query)
        {
            ResultT<List<GetAllocationByEmployeeDto>> result = await queryHandlers.GetEmployeeAllocations.Handle(query);

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
