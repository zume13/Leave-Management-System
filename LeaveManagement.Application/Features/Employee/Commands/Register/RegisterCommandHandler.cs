using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public class RegisterCommandHandler(IAuthService _service) : ICommandHandler<RegisterCommand, RegisterDto>
    {
        public Task<ResultT<RegisterDto>> Handle(RegisterCommand command, CancellationToken token = default)
        {
           
        }
    }
}
