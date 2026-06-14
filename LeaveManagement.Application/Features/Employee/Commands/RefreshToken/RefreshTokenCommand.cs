using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(string refreshToken) : ICommand<RefreshTokenDto>;
}
