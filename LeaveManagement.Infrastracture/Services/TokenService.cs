using LeaveManagement.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using LeaveManagement.Infrastructure.DateTimeProvider;


namespace LeaveManagement.Infrastructure.Services
{
    public class TokenService(IConfiguration _config) : ITokenService
    {
        public string GenerateAccessToken(Employee employee)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Sub, employee.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, employee.Email.Value),
                    new Claim(JwtRegisteredClaimNames.Name, employee.Name.Value),
                    new Claim("role", employee.Role.ToString())
                }),
                Audience = _config["Jwt:Audience"],
                Issuer = _config["Jwt:Issuer"],
                Expires = DateExpiry.accessTokenExpiry,
                SigningCredentials = credentials
            };

            var handler = new JsonWebTokenHandler();

            var token = handler.CreateToken(tokenDescriptor);

            return token;
        }

        public ResultT<RefreshToken> GenerateRefreshToken(Employee employee)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            if (string.IsNullOrEmpty(token))
                return InfrastractureErrors.TokenService.TokenGenerationFailed;

            return ResultT<RefreshToken>.Success(RefreshToken.Create(
                    id: Guid.NewGuid(),
                    token: token,
                    expiresAt: DateExpiry.refreshTokenExpiry,
                    employeeid: employee.Id
                ).Value);
        }
    }
}

