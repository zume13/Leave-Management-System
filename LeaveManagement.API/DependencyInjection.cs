
using LeaveManagement.API.Infrastracture;

namespace LeaveManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddControllers();
            
            return services;
        }
    }
}
