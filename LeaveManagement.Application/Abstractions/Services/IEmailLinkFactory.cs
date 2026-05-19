using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IEmailLinkFactory
    {
        ResultT<string> Create(EmailVerificationToken token);
    }
}
