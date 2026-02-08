using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Events;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Infrastructure.Events;
using LeaveManagement.Infrastructure.Persistence;
using LeaveManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IDomainEventDispatcher, DomainEventsDispatcher>();
            services.AddSingleton<IDomainEventTypeRegistry, DomainEventTypeRegistry>();
            services.AddScoped<IOutBoxMessageSerializer, OutboxMessageSerializer>();

            return services;
        }

    }
}
