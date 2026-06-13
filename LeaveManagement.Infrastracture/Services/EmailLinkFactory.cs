using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmailLinkFactory(IHttpContextAccessor _context, LinkGenerator _generator, IConfiguration _config) : IEmailLinkFactory
    {
        public ResultT<string> Create(EmailVerificationToken token)
        {
            var path = _generator.GetPathByName("verify", new { token = token.Id });   

            if(path == null) 
                return DomainErrors.General.InternalError;

            var link = $"{_config["App:BaseUrl"]}{path}";

            return ResultT<string>.Success(link);
        }
    }
}
