using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Handlers.LeaveAllocation;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastracture;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Threading.RateLimiting;
using static LeaveManagement.API.Constants.Constants;

namespace LeaveManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<EmployeeQueryHandlers>();
            services.AddScoped<EmployeeCommandHandlers>();
            services.AddScoped<AllocationCommandHandlers>();
            services.AddScoped<AllocationQueryHandlers>();
            services.AddScoped<RequestCommandHandlers>();
            services.AddScoped<RequestQueryHandlers>();
            services.AddScoped<TypeCommandHandlers>();
            services.AddScoped<TypeQueryHandlers>();

            services.AddRateLimiter(opt =>
            {
                opt.OnRejected = async (context, ct) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        error = "Too many requests. Please try again later."
                    }, ct);
                };

                opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
                    RateLimitPartition.GetSlidingWindowLimiter("Global", _ => new SlidingWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        AutoReplenishment = true,
                        PermitLimit = 1000,
                        QueueLimit = 5,
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

                opt.AddPolicy(RateLimits.Strict, httpContext =>
                {
                    var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                                 ?? httpContext.Connection.RemoteIpAddress?.ToString()
                                 ?? "Anonymous";

                    return RateLimitPartition.GetSlidingWindowLimiter(userId, _ => new SlidingWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        SegmentsPerWindow = 5,
                        QueueLimit = 0
                    });
                });

                opt.AddPolicy(RateLimits.PerUser, httpContext =>
                {
                    var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? httpContext.Connection.RemoteIpAddress?.ToString()
                            ?? "Anonymous";

                    return RateLimitPartition.GetSlidingWindowLimiter(userId, _ => new SlidingWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        AutoReplenishment = true,
                        PermitLimit = 60,
                        QueueLimit = 5,
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });

                });
            });

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
