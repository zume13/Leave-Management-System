
using LeaveManagement.Application.Models;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task<Result> SendEmailVerificationAsync(User user);
    }
}
