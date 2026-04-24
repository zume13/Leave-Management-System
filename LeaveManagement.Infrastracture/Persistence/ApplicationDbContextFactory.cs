using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LeaveManagement.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContextFactory() { }
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optBuilder.UseSqlServer("Server=thirdy\\SQLEXPRESS;Database=LeaveManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new ApplicationDbContext(optBuilder.Options, null!);
        }
    }
}
