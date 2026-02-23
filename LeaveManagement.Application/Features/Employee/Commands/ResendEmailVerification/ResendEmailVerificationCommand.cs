using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Commands.ResendEmailVerification
{
    public record ResendEmailVerificationCommand(string Email) : ICommand<VerifyEmailDto>;
}
