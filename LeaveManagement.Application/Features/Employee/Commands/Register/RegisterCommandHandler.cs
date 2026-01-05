using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Auth;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public class RegisterCommandHandler(IAuthService _service) : ICommandHandler<RegisterCommand, UserDto>
    {
        public Task<ResultT<UserDto>> Handle(RegisterCommand command, CancellationToken token = default)
        {
           
        }
    }
}
