using FluentEmail.Core;
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IFluentEmail _email;
        private readonly IEmailLinkFactory _factory;
        private readonly IApplicationDbContext _context;

        public EmailService(IConfiguration config, IFluentEmail email, IEmailLinkFactory factory, IApplicationDbContext context)
        {
            _config = config;
            _email = email;
            _factory = factory;
            _context = context;
        }

        public async Task<Result> SendEmailVerificationAsync(
            Guid employeeId,
            CancellationToken ct = default)
        {
            var employee = await _context.Employees.FindAsync(employeeId, ct); 

            if(employee is null)
                return Result.Failure(InfrastractureErrors.User.UserNotFound);

            var link = _factory.Create(EmailVerificationToken.Create(Guid.Parse(employee.VerificationToken!), employeeId).Value);

            var body = $@"<!DOCTYPE html>
                        <html>
                        <body style='font-family: Arial, sans-serif; background:#f4f4f4; padding:20px;'>
                            <table width='100%' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td align='center'>
                                        <table width='600' cellpadding='0' cellspacing='0'
                                               style='background:#ffffff;border-radius:8px;padding:40px;'>
                                            <tr>
                                                <td align='center'>
                                                    <h2>Email Verification</h2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p>Hello {employee.Name.Value},</p>

                                                    <p>
                                                        Thank you for registering. Please verify your
                                                        email address by clicking the button below.
                                                    </p>

                                                    <p style='text-align:center;margin:30px 0;'>
                                                        <a href='{link.Value}'
                                                           style='background:#2563eb;
                                                                  color:white;
                                                                  padding:12px 24px;
                                                                  text-decoration:none;
                                                                  border-radius:6px;'>
                                                            Verify Email
                                                        </a>
                                                    </p>

                                                    <p>
                                                        If the button doesn't work, use this link:
                                                    </p>

                                                    <p>
                                                        <a href='{link.Value}'>
                                                            {link.Value}
                                                        </a>
                                                    </p>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>";

            var result = await _email.To(employee.Email.Value)
                  .Subject("Email Verification")
                  .Body(body, isHtml: true)
                  .SendAsync();

            if(!result.Successful)
                return Result.Failure(InfrastractureErrors.General.InternalError);

            return Result.Success();
        }

        public async Task<Result> SendLeaveApprovedEmailAsync(string employeeName, string employeeEmail, string leaveName, CancellationToken ct = default)
        {
            var htmlBody = $@"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='UTF-8'>
                            <title>Leave Request Approved</title>
                        </head>
                        <body style='margin:0;padding:0;background-color:#f4f6f8;font-family:Arial,sans-serif;'>
                            <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f8;padding:30px 0;'>
                                <tr>
                                    <td align='center'>
                                        <table width='600' cellpadding='0' cellspacing='0'
                                               style='background:#ffffff;border-radius:8px;overflow:hidden;'>

                                            <!-- Header -->
                                            <tr>
                                                <td style='background:#28a745;padding:24px;text-align:center;color:#ffffff;'>
                                                    <h1 style='margin:0;font-size:24px;'>Leave Request Approved</h1>
                                                </td>
                                            </tr>

                                            <!-- Content -->
                                            <tr>
                                                <td style='padding:32px;color:#333333;line-height:1.6;'>
                                                    <p>Dear <strong>{employeeName}</strong>,</p>

                                                    <p>
                                                        We are pleased to inform you that your leave request has been
                                                        <strong>approved</strong>.
                                                    </p>

                                                    <table cellpadding='8' cellspacing='0'
                                                           style='width:100%;background:#f8f9fa;border:1px solid #e9ecef;border-radius:4px;'>
                                                        <tr>
                                                            <td><strong>Request ID:</strong></td>
                                                            <td>{leaveName}</td>
                                                        </tr>
                                                        <tr>
                                                            <td><strong>Employee Email:</strong></td>
                                                            <td>{employeeEmail}</td>
                                                        </tr>
                                                    </table>

                                                    <p style='margin-top:24px;'>
                                                        Please coordinate with your manager and team to ensure a smooth
                                                        transition of responsibilities during your absence.
                                                    </p>

                                                    <p>
                                                        If you have any questions, please contact the HR department.
                                                    </p>

                                                    <p>
                                                        Best regards,<br/>
                                                        <strong>Human Resources</strong>
                                                    </p>
                                                </td>
                                            </tr>

                                            <!-- Footer -->
                                            <tr>
                                                <td style='background:#f8f9fa;padding:16px;text-align:center;
                                                          color:#6c757d;font-size:12px;'>
                                                    This is an automated email. Please do not reply directly.
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>";

            var send = await _email.To(employeeEmail)
                .Subject("Leave Request Approved")
                .Body(htmlBody, isHtml: true)
                .SendAsync(ct);

            if(!send.Successful)
                return Result.Failure(InfrastractureErrors.General.InternalError);

            return Result.Success();
        }

        public async Task<Result> SendLeavedRejectedEmailAsync(string employeeName, string employeeEmail, string leaveName, string rejectionReason, CancellationToken ct = default)
        {

            var htmlBody = $@"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset=""UTF-8"">
                                <title>Leave Request Rejected</title>
                            </head>
                            <body style=""margin:0;padding:0;background-color:#f4f6f8;font-family:Arial,sans-serif;"">
                                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f4f6f8;padding:30px 0;"">
                                    <tr>
                                        <td align=""center"">
                                            <table width=""600"" cellpadding=""0"" cellspacing=""0""
                                                   style=""background:#ffffff;border-radius:8px;overflow:hidden;"">

                                                <!-- Header -->
                                                <tr>
                                                    <td style=""background:#dc3545;padding:24px;text-align:center;color:#ffffff;"">
                                                        <h1 style=""margin:0;font-size:24px;"">Leave Request Rejected</h1>
                                                    </td>
                                                </tr>

                                                <!-- Content -->
                                                <tr>
                                                    <td style=""padding:32px;color:#333333;line-height:1.6;"">
                                                        <p>Dear <strong>{employeeName}</strong>,</p>

                                                        <p>
                                                            We regret to inform you that your leave request has been
                                                            <strong>rejected</strong>.
                                                        </p>

                                                        <table cellpadding=""8"" cellspacing=""0""
                                                               style=""width:100%;background:#f8f9fa;border:1px solid #e9ecef;border-radius:4px;"">
                                                            <tr>
                                                                <td width=""180""><strong>Employee Email:</strong></td>
                                                                <td>{employeeEmail}</td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Leave Type:</strong></td>
                                                                <td>{leaveName}</td>
                                                            </tr>
                                                        </table>

                                                        <div style=""margin-top:24px;padding:16px;background:#fff3cd;border-left:4px solid #ffc107;border-radius:4px;"">
                                                            <strong>Reason for Rejection:</strong>
                                                            <p style=""margin:8px 0 0 0;"">
                                                                {rejectionReason}
                                                            </p>
                                                        </div>

                                                        <p style=""margin-top:24px;"">
                                                            If you believe additional information may support your request,
                                                            please contact your manager or the HR department.
                                                        </p>

                                                        <p>
                                                            You may also submit a new leave request if circumstances change.
                                                        </p>

                                                        <p>
                                                            Regards,<br/>
                                                            <strong>Human Resources</strong>
                                                        </p>
                                                    </td>
                                                </tr>

                                                <!-- Footer -->
                                                <tr>
                                                    <td style=""background:#f8f9fa;padding:16px;text-align:center;color:#6c757d;font-size:12px;"">
                                                        This is an automated email. Please do not reply directly.
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                            </html>";

            var send = await _email.To(employeeEmail)
                .Subject("Leave Request Rejected")
                .Body(htmlBody, isHtml: true)
                .SendAsync(ct);

            if(!send.Successful)
                return Result.Failure(InfrastractureErrors.General.InternalError);

            return Result.Success();
        }
    }
}
