using LeaveManagement.Application.AuthResponse;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<ResultT<AuthResult>> LoginAsync(string email, string password);
        Task<ResultT<AuthResult>> RegisterAsync(string email, string employeeName, string password, Guid deptId);
        Task<ResultT<AuthResult>> RefreshTokenAsync(string refreshToken);

    }
}
