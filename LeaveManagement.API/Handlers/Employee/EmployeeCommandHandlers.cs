using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee;
using LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee;

namespace LeaveManagement.API.Handlers.Employee
{
    public class EmployeeCommandHandlers
    {
            public ICommandHandler<LogInCommand, LogInDto> LogIn { get; }
            public ICommandHandler<RegisterCommand, RegisterDto> Register { get; }
            public ICommandHandler<RemoveEmployeeCommand, bool> Remove { get; }
            public ICommandHandler<UpdateEmployeeCommand, Guid> Update { get; }

            public EmployeeCommandHandlers(
                ICommandHandler<LogInCommand, LogInDto> _LogIn,
                ICommandHandler<RegisterCommand, RegisterDto> _Register,
                ICommandHandler<RemoveEmployeeCommand, bool> _Remove,
                ICommandHandler<UpdateEmployeeCommand, Guid> _Update)
            {
                LogIn = _LogIn;
                Register = _Register;
                Remove = _Remove;
                Update = _Update;
            }
        }
    }
