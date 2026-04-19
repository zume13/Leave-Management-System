using Scalar.AspNetCore;

namespace LeaveManagement.API.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static WebApplication UseScalar(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Leave Management API";
            });

            return app;
        }

    }
}
