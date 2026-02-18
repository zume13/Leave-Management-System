
using LeaveManagement.API.Infrastracture;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;

namespace LeaveManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LeaveManagementApi",
                    Description = "An Asp.Net Api for Leave Management"
                });
            });


            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddControllers();
            
            return services;
        }
    }
}
