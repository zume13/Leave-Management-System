using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmailLinkFactory(IHttpContextAccessor _context, LinkGenerator _generator, IConfiguration _config) : IEmailLinkFactory
    {
        public ResultT<string> Create(EmailVerificationToken token)
        {
            var baseUrl = "http://localhost:4200";

            var link = $"{baseUrl}/verify-email?token={token.Id.ToString()}";

            return ResultT<string>.Success(link);
        }
    }
}
