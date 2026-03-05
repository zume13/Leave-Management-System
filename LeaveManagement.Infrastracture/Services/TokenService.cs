using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;


namespace LeaveManagement.Infrastructure.Services
{
    public class TokenService(IConfiguration _config, UserManager<User> manager) : ITokenService
    {
        public async Task<string?> GenerateAccessToken(User user, DateTime expiry)
        {

            var roles = await manager.GetRolesAsync(user);

            if (roles.Count == 0)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.EmployeeName),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            claims.AddRange(roles.Select(
                        role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ResultT<RefreshToken> GenerateRefreshToken(User user, DateTime expiry)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            if (string.IsNullOrEmpty(token))
                return InfrastractureErrors.TokenService.TokenGenerationFailed;

            return ResultT<RefreshToken>.Success(RefreshToken.Create(
                    id: Guid.NewGuid(),
                    token: token,
                    expiresAt: expiry,
                    userId: user.Id
                ).Value);
        }
    }
}

