using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Employee;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.RotateRefreshToken
{
    public sealed class RefreshTokenCommandHandler(IApplicationDbContext _context, ITokenService _service) : ICommandHandler<RefreshTokenCommand, RefreshTokenDto>
    {
        public async Task<ResultT<RefreshTokenDto>> Handle(RefreshTokenCommand command, CancellationToken token = default)
        {
           var refreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == command.refreshToken && rt.RevokedAt == null);

            if (refreshToken is null)
                return ResultT<RefreshTokenDto>.Failure(Error.NotFound("RefreshToken.NotFound", "Refresh token was not found"));

            var employee = _context.Employees.FirstOrDefault(e => e.Id == refreshToken.EmployeeId);

            if (employee is null)
                return ResultT<RefreshTokenDto>.Failure(Error.NotFound("Employee.NotFound", "Employee associated with the refresh token was not found"));

            var newAccessToken = _service.GenerateAccessToken(employee);

            if (newAccessToken is null)
                return ApplicationErrors.General.InternalError;

            var newRefreshToken = _service.GenerateRefreshToken(employee);

            if (newRefreshToken.isFailure)
                return ApplicationErrors.General.InternalError;

            refreshToken.Revoke(newRefreshToken.Value.Token);

            await _context.RefreshTokens.AddAsync(newRefreshToken.Value);

            await _context.SaveChangesAsync();

            return ResultT<RefreshTokenDto>.Success(
                new RefreshTokenDto(newAccessToken, DateTime.UtcNow.AddMinutes(30), newRefreshToken.Value.Token, newRefreshToken.Value.ExpiresAt));
        }
    }
}
