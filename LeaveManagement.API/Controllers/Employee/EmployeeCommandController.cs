using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Client;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Commands.EmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.LogOut;
using LeaveManagement.Application.Features.Employee.Commands.Promote;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee;
using LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.RotateRefreshToken;
using LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;
using System.Security.Claims;


namespace LeaveManagement.API.Controllers.Employee
{
    [Route("leave-management/employee")]
    [ApiController]
    public class EmployeeCommandController(EmployeeCommandHandlers commandHandler) : ControllerBase
    {
        [AllowAnonymous]
        [EnableRateLimiting(RateLimit.PolicyName.Strict)]
        [HttpPost("auth/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
        {
            ResultT<RegisterDto> result = await commandHandler.Register.Handle(command);

            return result.Match<RegisterDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimit.PolicyName.Strict)]
        [HttpPost("auth/login")]
        public async Task<IActionResult> LogInAsync([FromBody] LogInCommand command)
        {
            ResultT<LogInDto> result = await commandHandler.LogIn.Handle(command);

            Console.WriteLine("Login endpoint hit");

            return result.Match<LogInDto, IActionResult>(success =>
            {
                Response.Cookies.Append(
                    "refreshToken",
                    result.Value.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = result.Value.RefreshTokenExpiration
                    });

                return Ok(
                    new {
                        accessToken = result.Value.Accesstoken,
                    });
            }, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await commandHandler.Remove.Handle(new RemoveEmployeeCommand(id));

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequest request)
        {
            var command = new UpdateEmployeeCommand(id, request.EmployeeName, request.Email);
            Result result = await commandHandler.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationCommand command)
        {
            Result result = await commandHandler.VerifyEmail.Handle(command);

            return result.Match<IActionResult>(Ok, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPost("resend-verification/{token}")]
        public async Task<IActionResult> ResendVerification(string token)
        {
            Result result = await commandHandler.ReVerifyEmail.Handle(new ResendEmailVerificationCommand(token));

            return result.Match<IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.AdminOnly)]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPost("promote")]
        public async Task<IActionResult> Promote([FromBody] PromoteCommand command)
        {
            Result result = await commandHandler.Promote.Handle(command);

            return result.Match<IActionResult>(Ok, CustomResults.Problem);
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPost("auth/refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return CustomResults.Problem(Result.Failure(Error.NotFound("RefreshToken.NotFound", "RefreshToken was not found")));
            
            ResultT<RefreshTokenDto> result = await commandHandler.RefreshToken.Handle(new RefreshTokenCommand(refreshToken));
            return result.Match<RefreshTokenDto, IActionResult>(success =>
            {
                Response.Cookies.Append(
                    "refreshToken",
                    result.Value.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = result.Value.RefreshTokenExpiration
                    });
                return Ok(
                    new
                    {
                        accessToken = result.Value.Accesstoken
                    });
            }, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
        [HttpPost("auth/logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            Console.WriteLine("hit");

            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var command = new LogOutCommand(Guid.Parse(employeeId));

            var result = await commandHandler.LogOut.Handle(command);

            return result.Match<IActionResult>(() =>
            {
                Response.Cookies.Delete("refreshToken");
                return NoContent();
            }, CustomResults.Problem);

        }
    }
}