using Microsoft.AspNetCore.Identity;
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
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddQuartz();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseScalar();
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
