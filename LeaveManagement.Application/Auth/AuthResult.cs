using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.AuthResponse
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public Error error { get; set; } = null!;
    }
}
