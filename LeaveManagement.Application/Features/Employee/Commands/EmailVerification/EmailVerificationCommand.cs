using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.Employee.Commands.EmailVerification
{
    public record EmailVerificationCommand(string token) : ICommand;
  
}
