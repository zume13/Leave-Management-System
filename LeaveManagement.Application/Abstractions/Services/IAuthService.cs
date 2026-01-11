using LeaveManagement.Application.Dto.Response.Auth;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<ResultT<LogInDto>> LoginAsync(string email, string password, CancellationToken ct = default);
        Task<ResultT<RegisterDto>> RegisterAsync(string email, string employeeName, string password, Guid deptId, CancellationToken ct = default);
        Task<ResultT<RefreshTokenAsyncDto>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);

    }
}
