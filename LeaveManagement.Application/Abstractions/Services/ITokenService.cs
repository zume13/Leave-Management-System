using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Result;
using System.Security.Claims;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Employee employee);
        ResultT<RefreshToken> GenerateRefreshToken(Employee employee);
    }
}
