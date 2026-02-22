using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.Application.Dto.Response.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveType
{
    [Authorize]
    [Route("LeaveManagement/LeaveType")]
    [ApiController]
    public class LeaveTypeQueryController(TypeQueryHandlers handlers) : ControllerBase
    {
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            ResultT<List<LeavesDto>> result = await handlers.GetAllLeaves.Handle(new GetAllLeavesQuery());

            return result.Match<List<LeavesDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ResultT<LeavesDto> result = await handlers.GetLeaveById.Handle(new GetLeaveByIdQuery(id));

            return result.Match<LeavesDto, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
