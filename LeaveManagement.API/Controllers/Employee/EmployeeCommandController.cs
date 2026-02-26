using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Commands.EmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee;
using LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;
using static LeaveManagement.API.Constants.Constants;


namespace LeaveManagement.API.Controllers.Employee
{
    [Authorize]
    [Route("LeaveManagement/Employee")]
    [ApiController]
    public class EmployeeCommandController(EmployeeCommandHandlers commandHandler) : ControllerBase
    {
        [AllowAnonymous]
        [EnableRateLimiting(RateLimits.Strict)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
        {
            ResultT<RegisterDto> result = await commandHandler.Register.Handle(command);

            return result.Match<RegisterDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimits.Strict)]
        [HttpPost("Login")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInCommand command)
        {
            ResultT<LogInDto> result = await commandHandler.LogIn.Handle(command);

            return result.Match<LogInDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [EnableRateLimiting(RateLimits.PerUser)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await commandHandler.Remove.Handle(new RemoveEmployeeCommand(id));

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [EnableRateLimiting(RateLimits.PerUser)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            command = command with { EmployeeId = id };
            Result result = await commandHandler.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [EnableRateLimiting(RateLimits.PerUser)]
        [AllowAnonymous]
        [HttpGet("Verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            ResultT<VerifyEmailDto> result = await commandHandler.VerifyEmail.Handle(new EmailVerificationCommand(token));

            return result.Match<VerifyEmailDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimits.PerUser)]
        [HttpPost("ResendVerification")]
        public async Task<IActionResult> ResendVerification([FromBody] string email)
        {
            ResultT<VerifyEmailDto> result = await commandHandler.ReVerifyEmail.Handle(new ResendEmailVerificationCommand(email));

            return result.Match<VerifyEmailDto, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
