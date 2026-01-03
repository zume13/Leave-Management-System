using LeaveManagement.Application.Models;
using SharedKernel.Shared;
using System.Security.Claims;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
        ResultT<ClaimsPrincipal> ValidateRefreshToken(string refreshToken);  
    }
}
