using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using LeaveManagement.Infrastructure.Persistence;
using LeaveManagement.Infrastructure.Identity;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Quartz;
using LeaveManagement.Infrastructure.BackgroundJobs;
using LeaveManagement.Application;
using LeaveManagement.API.Extensions;

namespace LeaveManagement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddPresentation();

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

            builder.Services.AddDbContext<ApplicationDbContext>((sp, opt) =>
            {
                var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                opt.UseSqlServer(connection).AddInterceptors(interceptor!);
            });

            builder.Services.AddQuartz(config =>
            {
                var JobKey = new JobKey(nameof(ProcessOutBoxMessageJob));

                config.AddJob<ProcessOutBoxMessageJob>(opt => opt.WithIdentity(JobKey))
                .AddTrigger(
                    trigger =>
                    trigger.ForJob(JobKey)
                           .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            await SeedData.InitializeAsync(scope.ServiceProvider);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerWithUI();
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
