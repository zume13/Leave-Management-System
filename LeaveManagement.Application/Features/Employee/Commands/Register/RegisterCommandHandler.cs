using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.Employee.Commands.Register
{
    public class RegisterCommandHandler(IAuthService service) : ICommandHandler<RegisterCommand, RegisterDto>
    {
        private readonly IAuthService _service = service;
        public Task<ResultT<RegisterDto>> Handle(RegisterCommand command, CancellationToken token = default)
        {
            return _service.RegisterAsync(command.Email, command.EmployeeName, command.Password, command.DepartmentId, token);
        }
    }
}
