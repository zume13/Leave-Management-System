using Swashbuckle.AspNetCore.SwaggerGen;    

namespace LeaveManagement.API.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static WebApplication UseSwaggerWithUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

    }
}
