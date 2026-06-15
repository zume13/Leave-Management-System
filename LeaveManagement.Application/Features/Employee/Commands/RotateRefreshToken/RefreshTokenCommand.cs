using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;

namespace LeaveManagement.Application.Features.Employee.Commands.RotateRefreshToken
{
    public sealed record RefreshTokenCommand(string refreshToken) : ICommand<RefreshTokenDto>;
}
