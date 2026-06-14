using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Employee;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler(IApplicationDbContext _context, IAuthService _service) : ICommandHandler<RefreshTokenCommand, RefreshTokenDto>
    {
        public async Task<ResultT<RefreshTokenDto>> Handle(RefreshTokenCommand command, CancellationToken token = default)
        {
           var refreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == command.refreshToken);

            if (refreshToken is null)
                return ResultT<RefreshTokenDto>.Failure(Error.NotFound("RefreshToken.NotFound", "Refresh token was not found"));

            var employee = _context.Employees.FirstOrDefault(e => e.Id == refreshToken.EmployeeId);

            if (employee is null)
                return ResultT<RefreshTokenDto>.Failure(Error.NotFound("Employee.NotFound", "Employee associated with the refresh token was not found"));

            var newAccessToken = employee.GenerateAccessToken();
            var newRefreshToken = employee.GenerateRefreshToken();

            // Update the refresh token in the database
            refreshToken.Token = newRefreshToken;
            refreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7); // Set expiry date for the new refresh token
            _context.SaveChanges();
            var refreshTokenDto = new RefreshTokenDto(newAccessToken, newRefreshToken);
            return Task.FromResult(ResultT<RefreshTokenDto>.Success(refreshTokenDto));
        }
    }
}
