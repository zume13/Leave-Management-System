using LeaveManagement.Application.Abstractions.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailVerificationAsync(
            string employeeName,
            string employeeEmail,
            string verificationToken,
            CancellationToken ct = default)
        {
            var message = CreateMessage(
                employeeName,
                employeeEmail,
                "Email Verification",
                $"Please verify your email by clicking on the following link: " +
                $"https://ZLM.com/verify?token={verificationToken}");

            await SendAsync(message, ct);
        }

        public async Task SendLeaveApprovedEmailAsync(
            string employeeName,
            string employeeEmail,
            string admin,
            CancellationToken ct = default)
        {
            var message = CreateMessage(
                employeeName,
                employeeEmail,
                "Approved Leave",
                $"Your requested leave was approved by {admin} on {DateTime.UtcNow:yyyy-MM-dd HH:mm}");

            await SendAsync(message, ct);
        }

        private MimeMessage CreateMessage(
            string employeeName,
            string employeeEmail,
            string subject,
            string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                "Leave Management",
                _config["EmailConfiguration:From"]
                    ?? throw new InvalidOperationException("Email 'From' not configured")));

            message.To.Add(new MailboxAddress(employeeName, employeeEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            return message;
        }

        private async Task SendAsync(MimeMessage message, CancellationToken ct)
        {
            if (!int.TryParse(_config["EmailConfiguration:Port"], out var port))
                throw new InvalidOperationException("Invalid SMTP port configuration");

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _config["EmailConfiguration:SmtpServer"]
                    ?? throw new InvalidOperationException("SMTP server not configured"),
                port,
                true,
                ct);

            await client.AuthenticateAsync(
                _config["EmailConfiguration:Username"],
                _config["EmailConfiguration:Password"],
                ct);

            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
