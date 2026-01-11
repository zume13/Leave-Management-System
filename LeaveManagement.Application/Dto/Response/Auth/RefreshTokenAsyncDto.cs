using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Application.Dto.Response.Auth
{
    public class RefreshTokenAsyncDto
    {
        public string? AccessToken { get; set; } 
        public string? RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
