using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.Department;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Department.Queries
{
    public class GetDepartmentsQueryHandler(IApplicationDbContext _context) : IQueryHandler<GetDepartmentsQuery, List<DepartmentDto>>
    {
        public async Task<ResultT<List<DepartmentDto>>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            int pageSize = request.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(request.pageSize);
            int pageNumber = Math.Max(1, request.pageNumber);

            var departments = await _context.Departments.
                    Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

            if (departments.Count == 0)
                return ApplicationErrors.General.InternalError;

            var departmentsDto = departments.Select(d => new DepartmentDto
            ( 
                d.Id, 
                d.DepartmentName.Value
            )).ToList();

            return ResultT<List<DepartmentDto>>.Success(departmentsDto);
        }
    }

}
