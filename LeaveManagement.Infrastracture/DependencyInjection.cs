using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Infrastructure.Events;
using LeaveManagement.Infrastructure.Persistence;
using LeaveManagement.Infrastructure.Persistence.Interceptors;
using LeaveManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeaveManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ConvertDomainEventsToOutboxMessagesInterceptor>();
            services.AddScoped<IDomainEventDispatcher, DomainEventsDispatcher>();
            services.AddSingleton<IDomainEventTypeRegistry, DomainEventTypeRegistry>();
            services.AddScoped<IOutBoxMessageSerializer, OutboxMessageSerializer>();

            services.AddDbContext<ApplicationDbContext>((sp, opt) =>
            {

                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                
            });
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddHttpContextAccessor();

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(config["Smtp:Email"], config["Smtp:Password"]),
                EnableSsl = true,
            };

            services.AddFluentEmail(config["Smtp:Email"], config["Smtp:From"])
                    .AddSmtpSender(smtpClient);

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailLinkFactory, EmailLinkFactory>();
            services.AddScoped<SeederService>();

            services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
            
            return services;
        }

    }
}
