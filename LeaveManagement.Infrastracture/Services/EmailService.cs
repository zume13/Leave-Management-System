using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SharedKernel.Shared;

namespace LeaveManagement.Infrastracture.Services
{
    public class EmailService(IConfiguration _config) : IEmailService
    {

        public async Task<Result> SendEmailVerificationAsync(User user)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Leave Management", _config["EmailConfiguration:From"]));
            emailMessage.To.Add(new MailboxAddress(user.EmployeeName.ToString(), user.Email));
            emailMessage.Subject = "Email Verification";
            emailMessage.Body = new TextPart("plain")
            {
                Text = $"Please verify your email by clicking on the following link: https://yourdomain.com/verify?token={user.verificationToken}"
            };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_config["EmailConfiguration:SmtpServer"], int.Parse(_config["EmailConfiguration:Port"]!), true);
                await client.AuthenticateAsync(_config["EmailConfiguration:Username"], _config["EmailConfiguration:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("EmailService.SendEmailVerificationAsync", $"Failed to send email: {ex.Message}"));
            }

            return Result.Success();
        }
    }
}
