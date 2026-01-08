using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Infrastructure.Messaging;
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
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

    }
}
