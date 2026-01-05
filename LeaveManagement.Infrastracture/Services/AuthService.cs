using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.AuthResponse;
using LeaveManagement.Application.Features.Employee.Commands.LogIn;
using LeaveManagement.Application.Features.Employee.Commands.Register;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Infrastracture.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<User> user, SignInManager<User> sign, ITokenService token)
        {
            _userManager = user;
            _signInManager = sign;
            _tokenService = token;  
        }   
        public Task<ResultT<AuthResult>> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ResultT<AuthResult>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<ResultT<AuthResult>> RegisterAsync(string email, string employeeName, string password, Guid deptId)
        {
            var existingUser = _userManager.FindByEmailAsync(email);

            if(existingUser is null)
        }
    }
}
