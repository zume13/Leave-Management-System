using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Dto.Response;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Models;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared;

namespace LeaveManagement.Infrastracture.Services
{
    public class AuthService(UserManager<User> _userManager, IApplicationDbContext _context, IEmailService _emailService, ITokenService _token) : IAuthService
    {
        
        public async Task<ResultT<LogInDto>> LoginAsync(string email, string password)
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

            _context.RefreshTokens.Add(refreshToken.Value);

            return ResultT<LogInDto>.Success(new LogInDto
            {
                IsSuccessful = true,
                UserId = user.Id,
                Accesstoken = token,
                RefreshToken = refreshToken.Value.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(15), 
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7) 
            });
        }

        public async Task<ResultT<RefreshTokenAsyncDto>> RefreshTokenAsync(string refreshToken)
        {
            if(string.IsNullOrWhiteSpace(refreshToken))
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidInput);

            var oldRefreshToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
            
            if(oldRefreshToken is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.TokenService.InvalidRefreshToken);

            var user = await _userManager.FindByIdAsync(oldRefreshToken.UserId.ToString());

            if(user is null)
                return ResultT<RefreshTokenAsyncDto>.Failure(InfrastractureErrors.User.UserNotFound);

            var newRefreshToken = _token.GenerateRefreshToken(user);

            return ResultT<RefreshTokenAsyncDto>.Success(new RefreshTokenAsyncDto());
        }

        public async Task<ResultT<RegisterDto>> RegisterAsync(string email, string employeeName, string password, Guid deptId)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.UserEmailExists);

            var user = new User
            {
                UserName = email,
                Email = email,
                EmployeeName = Name.Create(employeeName).Value,
                isEmailVerified = false,
                verificationToken = Guid.NewGuid().ToString(),
                tokenExpiration = DateTime.UtcNow.AddDays(1)
            };

            var createUserResult = await _userManager.CreateAsync(user, password);

            if (!createUserResult.Succeeded)
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.User.FailedRegistry);

            var newMail = Email.Create(user.Email).Value;
            var employee = Employee.Create(user.EmployeeName, newMail, deptId, user.Id);

            if (employee.isFailure)
                return ResultT<RegisterDto>.Failure(DomainErrors.Employee.NullEmployee);

            await _context.Employees.AddAsync(employee.Value);

            var emailResult = await _emailService.SendEmailVerificationAsync(user);
             
            if (emailResult.isFailure)
            {
                await _userManager.DeleteAsync(user);
                _context.Employees.Remove(employee.Value);
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.Email.FailedToSendVerificationEmail);
            }

            var registerDto = new RegisterDto
            {
                Success = true,
                Message = "User registered successfully. Please check your inbox and verify your email.",
                UserId = user.Id,
                Email = user.Email,
                IsEmailVerified = user.isEmailVerified
            };

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                await _userManager.DeleteAsync(user);
                _context.Employees.Remove(employee.Value);
                return ResultT<RegisterDto>.Failure(InfrastractureErrors.General.InternalError);
            }

            return ResultT<RegisterDto>.Success(registerDto);
        }
        //add verification endpoint
    }
}
