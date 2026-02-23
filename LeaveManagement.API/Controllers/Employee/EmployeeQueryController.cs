using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment;
using LeaveManagement.Application.Features.Employee.Queries.ListEmployees;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.Employee
{
    [Route("LeaveManagement/Employee")]
    [ApiController]
    public class EmployeeQueryController(EmployeeQueryHandlers queryHandler) : ControllerBase
    {
        [HttpGet("All")]
        public async Task<IActionResult> GetEmployees()
        {
            ResultT<List<EmployeeDto>> result =
                await queryHandler.GetAll.Handle(new GetAllEmployeesQuery());

            return result.Match<List<EmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ResultT<EmployeeDto> result =
                await queryHandler.GetById.Handle(new GetEmployeeByIdQuery(id));

            return result.Match<EmployeeDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Department/{deptId}")]
        public async Task<IActionResult> GetByDepartment(Guid deptId)
        {
            ResultT<List<GetEmployeesByDepartmentDto>> result =
                await queryHandler.GetByDepartment.Handle(new GetEmployeesByDepartmentQuery(deptId));

            return result.Match<List<GetEmployeesByDepartmentDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
