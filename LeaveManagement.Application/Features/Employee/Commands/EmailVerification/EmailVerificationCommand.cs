using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    public record EmailVerificationCommand(string token) : ICommand<VerifyEmailDto>;
  
}
