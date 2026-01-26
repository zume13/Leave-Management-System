using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Auth;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public sealed class LogInCommandHandler(IAuthService service) : ICommandHandler<LogInCommand, LogInDto>
    {
        private readonly IAuthService _service = service;
        public async Task<ResultT<LogInDto>> Handle(LogInCommand command, CancellationToken token = default)
        {
            return await _service.LoginAsync(command.Email, command.Password, token);
        }
    }
}
