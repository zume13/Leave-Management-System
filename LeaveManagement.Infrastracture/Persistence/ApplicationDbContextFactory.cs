using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace LeaveManagement.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContextFactory() { }
        public ApplicationDbContext CreateDbContext(string[] args)
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LeaveManagement.API"))
            .AddJsonFile("appsettings.json")
            .Build();

            var optBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optBuilder.UseSqlServer(config["DefaultConnection"]!);

            return new ApplicationDbContext(optBuilder.Options, null!);
        }
    }
}
