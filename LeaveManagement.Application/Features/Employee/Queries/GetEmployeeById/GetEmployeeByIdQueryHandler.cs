using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;


namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployee
{
    internal sealed class GetEmployeeByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        IApplicationDbContext _context = context;
        public async Task<ResultT<EmployeeDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == query.EmployeeId)
                .Join(_context.Departments.AsNoTracking(),
                      e => e.DeptId,
                      d => d.Id,
                      (e, d) => new EmployeeDto(
                        e.Id, 
                        e.Name.Value,
                        e.Email.Value, 
                        e.Status, 
                        d.DepartmentName.Value))
                .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound;

            return ResultT<EmployeeDto>.Success(employee);
        }
    }
}
