using Microsoft.AspNetCore.Identity;
using LeaveManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Application;
using LeaveManagement.API.Extensions;
using LeaveManagement.Infrastructure.Services;

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
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddBackgroundJobs();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
               app.UseScalar();
            }

            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<SeederService>();

            await seeder.SeedAsync();

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAngular");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
    }
}
