using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee;
using LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment;
using LeaveManagement.Application.Features.Employee.Queries.ListEmployees;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;


namespace LeaveManagement.API.Controllers
{
    [Route("LeaveManagement/[controller]")]
    [ApiController]
    public class EmployeeController(
        EmployeeCommandHandlers commandHandler,
        EmployeeQueryHandlers queryHandler) 
        : ControllerBase
    {
        [HttpGet("all")]
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

        [HttpGet("department/{deptId}")]
        public async Task<IActionResult> GetByDepartment(Guid deptId)
        {
            ResultT<List<GetEmployeesByDepartmentDto>> result = 
                await queryHandler.GetByDepartment.Handle(new GetEmployeesByDepartmentQuery(deptId));

            return result.Match<List<GetEmployeesByDepartmentDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
        {
            ResultT<RegisterDto> result = await commandHandler.Register.Handle(command);

            return result.Match<RegisterDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInCommand command)
        {
            ResultT<LogInDto> result = await commandHandler.LogIn.Handle(command);

            return result.Match<LogInDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await commandHandler.Remove.Handle(new RemoveEmployeeCommand(id));

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            command = command with { EmployeeId = id };
            Result result = await commandHandler.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }
    }
}
