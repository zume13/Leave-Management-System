using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment;
using LeaveManagement.Application.Features.Employee.Queries.ListEmployees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.Employee
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("leave-management/employee")]
    [ApiController]
    public class EmployeeQueryController(EmployeeQueryHandlers queryHandler) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("all")]
        public async Task<IActionResult> GetEmployees([FromQuery] GetAllEmployeesQuery query)
        {
            ResultT<List<EmployeeDto>> result = await queryHandler.GetAll.Handle(query);

            return result.Match<List<EmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("{employeeId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid employeeId)
        {
            ResultT<EmployeeDto> result = await queryHandler.GetById.Handle(new GetEmployeeByIdQuery(employeeId));

            return result.Match<EmployeeDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("in-department/{deptId}")]
        public async Task<IActionResult> GetByDepartment([FromRoute] Guid deptId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetEmployeesByDepartmentDto>> result = await queryHandler.GetByDepartment.Handle(new GetEmployeesByDepartmentQuery(deptId, pageSize, pageNumber));

            return result.Match<List<GetEmployeesByDepartmentDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
