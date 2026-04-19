using LeaveManagement.API.Constants;
using LeaveManagement.API.Handlers.Employee;
using LeaveManagement.API.Handlers.LeaveAllocation;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastracture;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Threading.RateLimiting;

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

                opt.AddPolicy(RateLimit.PolicyName.Strict, httpContext =>
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

                opt.AddPolicy(RateLimit.PolicyName.PerUser, httpContext =>
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

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowSwaggerUI", policy =>
                {
                    policy.WithOrigins("https://localhost:7215")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Auth.Policies.AdminOnly, policy => policy.RequireRole(Auth.Roles.Admin));

                opt.AddPolicy(Auth.Policies.ManagerAndAbove, policy =>
                    policy.RequireRole(Auth.Roles.Admin, Auth.Roles.Manager));

                opt.AddPolicy(Auth.Policies.EmployeeAndAbove, policy =>
                    policy.RequireRole(Auth.Roles.Admin, Auth.Roles.Manager, Auth.Roles.Employee));
            });

            services.AddOpenApi(opt => 
                opt.AddDocumentTransformer((doc, context, ct) =>
                {
                    doc.Components ??= new();
                    doc.Components.SecuritySchemes!.Add("Bearer", new OpenApiSecurityScheme 
                    { 
                        Type = SecuritySchemeType.Http,
                        Description = "A Leave Management API secured with JWT Bearer Authentication",
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });
                    return Task.CompletedTask;
                }));

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddControllers();
            
            return services;
        }
    }
}
