using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmailLinkFactory(IHttpContextAccessor _context, LinkGenerator _generator) : IEmailLinkFactory
    {
        public ResultT<string> Create(EmailVerificationToken token)
        {
            var link = _generator.GetUriByName(_context.HttpContext!, "verify", new { token = token.Id });   

            if(link == null) 
                return DomainErrors.General.InternalError;

            return ResultT<string>.Success(link);
        }
    }
}
