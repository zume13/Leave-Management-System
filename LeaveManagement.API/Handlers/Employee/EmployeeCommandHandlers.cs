using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Commands.EmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Promote;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee;
using LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification;
using LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee;

namespace LeaveManagement.API.Handlers.Employee
{
    public class EmployeeCommandHandlers
    {
            public ICommandHandler<LogInCommand, LogInDto> LogIn { get; }
            public ICommandHandler<RegisterCommand, RegisterDto> Register { get; }
            public ICommandHandler<RemoveEmployeeCommand> Remove { get; }
            public ICommandHandler<UpdateEmployeeCommand> Update { get; }
            public ICommandHandler<EmailVerificationCommand> VerifyEmail { get; }
            public ICommandHandler<ResendEmailVerificationCommand> ReVerifyEmail { get; }
            public ICommandHandler<PromoteCommand, PromoteEmployeeDto> Promote { get; }


        public EmployeeCommandHandlers(
                ICommandHandler<LogInCommand, LogInDto> _LogIn,
                ICommandHandler<RegisterCommand, RegisterDto> _Register,
                ICommandHandler<RemoveEmployeeCommand> _Remove,
                ICommandHandler<UpdateEmployeeCommand> _Update,
                ICommandHandler<EmailVerificationCommand> _VerifyEmail,
                ICommandHandler<ResendEmailVerificationCommand> _ReVerifyEmail,
                ICommandHandler<PromoteCommand, PromoteEmployeeDto> _Promote)
            {
                LogIn = _LogIn;
                Register = _Register;
                Remove = _Remove;
                Update = _Update;
                VerifyEmail = _VerifyEmail;
                ReVerifyEmail = _ReVerifyEmail;
                Promote = _Promote;
        }
        }
    }
