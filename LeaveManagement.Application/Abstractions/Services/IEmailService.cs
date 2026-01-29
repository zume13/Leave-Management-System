using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(
            string employeeName,
            string employeeEmail,
            string verificationToken,
            CancellationToken ct = default);

        Task SendLeaveApprovedEmailAsync(
            string employeeName,
            string employeeEmail,
            string admin,
            CancellationToken ct = default);
    }
}
