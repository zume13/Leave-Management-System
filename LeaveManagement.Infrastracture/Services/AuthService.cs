using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LeaveManagement.Infrastructure.Services
{
    public class AuthService(UserManager<User> _userManager, IApplicationDbContext _context, ITokenService _token) : IAuthService
    {
        
        public async Task<ResultT<LogInDto>> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.User.InvalidCredentials);

            if (user.isEmailVerified == false)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.Email.EmailNotVerified);

            if (!await _userManager.CheckPasswordAsync(user, password))
                return ResultT<LogInDto>.Failure(InfrastractureErrors.User.InvalidCredentials);

            var token = _token.GenerateAccessToken(user);

            if(token is null)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.General.InternalError);

            var refreshToken = _token.GenerateRefreshToken(user);

            if(refreshToken.isFailure)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.General.InternalError);

            await _context.RefreshTokens.AddAsync(refreshToken.Value);

            await _context.SaveChangesAsync(ct);

            return ResultT<LogInDto>.Success(new LogInDto
            {
                IsSuccessful = true,
                UserId = user.Id,
                Accesstoken = token,
                RefreshToken = refreshToken.Value.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30), 
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7) 
            });
        }

        public async Task<ResultT<RefreshTokenAsyncDto>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(refreshToken))
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidInput);

            var principalResult = _token.ValidateRefreshToken(refreshToken);

            if (principalResult.isFailure)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            var userId = principalResult.Value.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var jti = principalResult.Value.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (userId is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.CredentialsError);

            if(jti is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.CredentialsError);

            var oldRefreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Id == Guid.Parse(jti) && rt.UserId == userId && rt.RevokedAt == null);
            
            if(oldRefreshToken is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            if(oldRefreshToken.IsExpired())
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            if(!oldRefreshToken.IsActive())
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.TokenReused);

            var user = await _userManager.FindByIdAsync(oldRefreshToken.UserId.ToString());

            if(user is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.User.UserNotFound);
            
            var newRefreshToken = _token.GenerateRefreshToken(user);
            var newAccessToken = _token.GenerateAccessToken(user);

            if(newRefreshToken.isFailure || newAccessToken is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.TokenGenerationFailed);

            try
            {
                oldRefreshToken.Revoke(newRefreshToken.Value.Token);
                _context.RefreshTokens.Add(newRefreshToken.Value);

                await _context.SaveChangesAsync(ct);
            }
            catch(Exception)
            {
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.General.InternalError);
            }

            return ResultT<RefreshTokenAsyncDto>.Success(new RefreshTokenAsyncDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Value.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(30),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            });
        }

        public async Task<ResultT<RegisterDto>> RegisterAsync(string email, string employeeName, string password, Guid deptId, CancellationToken ct = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null)
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.UserEmailExists);

            var user = new User
            {
                UserName = email,
                Email = email,
                EmployeeName = employeeName,
                isEmailVerified = false,
                verificationToken = Guid.NewGuid().ToString(),
                tokenExpiration = DateTime.UtcNow.AddDays(1)
            };

            try
            {
                var createUserResult = await _userManager.CreateAsync(user, password);

                if (!createUserResult.Succeeded)
                    return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.FailedRegistry);

                var newMail = Email.Create(user.Email).Value;
                var employee = Employee.Create(Name.Create(user.EmployeeName).Value, newMail, deptId, user.Id, user.verificationToken);

                if (employee.isFailure)
                {
                    await _userManager.DeleteAsync(user);
                    return ResultT<RegisterDto>.Failure(DomainErrors.Employee.NullEmployee);
                }

                await _context.Employees.AddAsync(employee.Value, ct);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                await _userManager.DeleteAsync(user);
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.General.InternalError);
            }

            return ResultT<RegisterDto>.Success(new RegisterDto
            {
                Success = true,
                Message = "User registered successfully. Please check your inbox and verify your email.",
                UserId = user.Id,
                Email = user.Email,
                IsEmailVerified = user.isEmailVerified
            });
        }
    }
}
