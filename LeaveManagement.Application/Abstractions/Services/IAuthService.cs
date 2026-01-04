using LeaveManagement.Application.Auth;
using LeaveManagement.Application.AuthResponse;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IAuthService
    {
        Task<ResultT<AuthResult>> LoginAsync(LogInDto login);
  
    }
}
