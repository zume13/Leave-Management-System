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
            services.AddDbContext<ApplicationDbContext>((sp, opt) =>
            {
                var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                opt.UseSqlServer(config.GetConnectionString("DefaultConnection")).AddInterceptors(interceptor!);
                
            });

            services.AddScoped<IEmailLinkFactory, EmailLinkFactory>();

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());


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

            services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
            services.AddScoped<IDomainEventDispatcher, DomainEventsDispatcher>();
            services.AddSingleton<IDomainEventTypeRegistry, DomainEventTypeRegistry>();
            services.AddScoped<IOutBoxMessageSerializer, OutboxMessageSerializer>();

            return services;
        }

    }
}
