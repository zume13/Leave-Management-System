using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Result;
using System.Security.Claims;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        ResultT<RefreshToken> GenerateRefreshToken(User user);
        ResultT<ClaimsPrincipal> ValidateRefreshToken(string refreshToken);  
    }
}
