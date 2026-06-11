using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task<Result> SendEmailVerificationAsync(
            Guid employeeId,
            CancellationToken ct = default);

        Task<Result> SendLeaveApprovedEmailAsync(
            string employeeName,
            string employeeEmail,
            string leaveName,
            CancellationToken ct = default);

        Task<Result> SendLeavedRejectedEmailAsync(
            string employeeName,
            string employeeEmail,
            string leaveName,
            string rejectionReason,
            CancellationToken ct = default);
    }
}
