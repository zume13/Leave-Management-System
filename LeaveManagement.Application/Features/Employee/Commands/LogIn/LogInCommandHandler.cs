using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.LogIn
{
    public class LogInCommandHandler(IAuthService service) : ICommandHandler<LogInCommand, LogInDto>
    {
        private readonly IAuthService _service = service;
        public async Task<ResultT<LogInDto>> Handle(LogInCommand command, CancellationToken token = default)
        {
            return await _service.LoginAsync(command.Email, command.Password, token);
        }
    }
}
