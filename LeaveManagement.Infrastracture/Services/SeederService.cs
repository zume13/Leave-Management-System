
using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeaveManagement.Infrastructure.Services
{
    public class SeederService(IApplicationDbContext _context, IPasswordHasher<Employee> _hasher, IConfiguration _config) 
    {

        public async Task SeedAsync(CancellationToken token = default)
        {
            await SeedDepartmentsAsync(token);
            await SeedAdminAsync(token);
        }

        public async Task SeedDepartmentsAsync(CancellationToken token = default)
        { 
            if(await _context.Departments.AnyAsync(token))
                return;

            var adminDeptId = Guid.Parse(_config["AdminCreds:Department"] ?? throw new InvalidOperationException("AdminCreds:Department is missing"));
            await _context.Departments.AddRangeAsync(
             Department.Create(adminDeptId, Name.Create("Admin").Value).Value,
             Department.Create(Guid.NewGuid(), Name.Create("Human Resources").Value).Value,
             Department.Create(Guid.NewGuid(), Name.Create("Finance").Value).Value,
             Department.Create(Guid.NewGuid(), Name.Create("Engineering").Value).Value,
             Department.Create(Guid.NewGuid(), Name.Create("Marketing").Value).Value);

            await _context.SaveChangesAsync();
        }

        public async Task SeedAdminAsync(CancellationToken token = default)
        {
            var email = Email.Create(_config["AdminCreds:Email"] ?? throw new InvalidOperationException("AdminCreds:Email is missing")).Value;
            var name = Name.Create(_config["AdminCreds:Name"] ?? throw new InvalidOperationException("AdminCreds:Name is missing"));
            var hashedPassword = _hasher.HashPassword(null!, _config["AdminCreds:Password"] ?? throw new InvalidOperationException("AdminCreds:Password is missing"));
            var deptId = Guid.Parse(_config["AdminCreds:Department"] ?? throw new InvalidOperationException("AdminCreds:Department is missing"));

            var existingAdmin = await _context.Employees.FirstOrDefaultAsync(e => e.Email.Value == email.Value, token);

            if(existingAdmin is not null)
                return;

            var admin = Employee.Create(name.Value, email, deptId, null!, hashedPassword).Value;

            admin.AssignRole(Role.Admin);

            await _context.Employees.AddAsync(admin);

            await _context.SaveChangesAsync();
            
        }

    }
}
