using Microsoft.AspNetCore.Identity;
using LeaveManagement.Infrastructure.Identity;
using LeaveManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}
