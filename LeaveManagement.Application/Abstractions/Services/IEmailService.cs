
using LeaveManagement.Application.Models;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task<Result> SendEmailVerificationAsync(string EmployeeName, string EmployeeEmail, string VerificationToken);
    }
}
