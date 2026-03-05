using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Result;
using System.Security.Claims;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<string?> GenerateAccessToken(User user, DateTime expiry);
        ResultT<RefreshToken> GenerateRefreshToken(User user, DateTime expiry);
    }
}
