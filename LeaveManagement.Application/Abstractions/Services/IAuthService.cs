using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<ResultT<LogInDto>> LoginAsync(string email, string password);
        Task<ResultT<RegisterDto>> RegisterAsync(string email, string employeeName, string password, Guid deptId);
        Task<Result> RefreshTokenAsync(string refreshToken);

    }
}
