using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response.Auth;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using LeaveManagement.Infrastructure.DateTimeProvider;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Infrastructure.Services
{
    public class AuthService(IApplicationDbContext _context, ITokenService _token, IPasswordHasher<Employee> _hasher) : IAuthService
    {
        
        public async Task<ResultT<LogInDto>> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email.Value == email, ct);

            if (employee is null)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.User.InvalidCredentials);

            var verificationResult = _hasher.VerifyHashedPassword(employee, employee.HashedPassword, password);

            if (verificationResult == PasswordVerificationResult.Failed)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.User.InvalidCredentials);

            var token = _token.GenerateAccessToken(employee);

            if(token is null)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.General.InternalError);

            var refreshToken = _token.GenerateRefreshToken(employee);

            if(refreshToken.isFailure)
                return ResultT<LogInDto>.Failure(InfrastractureErrors.General.InternalError);

            await _context.RefreshTokens.AddAsync(refreshToken.Value);

            await _context.SaveChangesAsync(ct);

            return ResultT<LogInDto>.Success(new LogInDto
            {
                IsSuccessful = true,
                Accesstoken = token,
                RefreshToken = refreshToken.Value.Token,
                AccessTokenExpiration = DateExpiry.accessTokenExpiry, 
                RefreshTokenExpiration = DateExpiry.refreshTokenExpiry 
            });
        }
        public async Task<ResultT<RefreshTokenAsyncDto>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(refreshToken))
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidInput);

            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked, ct);
            
            if(oldRefreshToken is null || oldRefreshToken.IsRevoked)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            if(oldRefreshToken.IsExpired())
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            if(!oldRefreshToken.IsActive())
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.TokenReused);

            var employee = await _context.Employees.FindAsync(oldRefreshToken.EmployeeId, ct);

            if(employee is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.User.InvalidCredentials);

            var newRefreshToken = _token.GenerateRefreshToken(employee);
            var newAccessToken =  _token.GenerateAccessToken(employee);

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
                AccessTokenExpiration = DateExpiry.accessTokenExpiry,
                RefreshTokenExpiration = DateExpiry.refreshTokenExpiry
            });
        }
        public async Task<ResultT<RegisterDto>> RegisterAsync(string email, string employeeName, string password, Guid deptId, CancellationToken ct = default)
        {

            using var transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                var existingUser = await _context.Employees.FirstOrDefaultAsync(e => e.Email.Value == email, ct);
                if (existingUser is not null)
                    return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.UserEmailExists);

                var newMail = Email.Create(email).Value;

                if (newMail is null)
                {
                    return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.FailedRegistry);
                }

                var hashedpass = _hasher.HashPassword(null!, password);

                var verificationToken = Guid.NewGuid();

                var employee = Employee.Create(Name.Create(employeeName).Value, newMail, deptId, verificationToken.ToString(), hashedpass);

                if (employee.isFailure)
                {
                    return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.FailedRegistry);
                }

                var createdToken = EmailVerificationToken.Create(verificationToken, employee.Value.Id);

                if(createdToken.isFailure)
                    return ResultT<RegisterDto>.Failure(createdToken.Error);

                employee.Value.AssignRole(Role.Employee);
                await _context.EmailVerificationTokens.AddAsync(createdToken.Value, ct);
                await _context.Employees.AddAsync(employee.Value, ct);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return ResultT<RegisterDto>.Success(new RegisterDto
                {
                    Email = newMail.Value
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }   
            
        }
    }
}
