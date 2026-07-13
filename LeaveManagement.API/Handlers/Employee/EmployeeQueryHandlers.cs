using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Department;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Department.Queries;
using LeaveManagement.Application.Features.Employee.Queries.GetDashboardData;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeeByName;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment;
using LeaveManagement.Application.Features.Employee.Queries.ListEmployees;

namespace LeaveManagement.API.Handlers.Employee
{
    public class EmployeeQueryHandlers
    {
        public IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>> GetAll { get; }
        public IQueryHandler<GetEmployeeByIdQuery, EmployeeDto> GetById { get; }
        public IQueryHandler<GetEmployeesByDepartmentQuery, List<EmployeeDto>> GetByDepartment { get; }
        public IQueryHandler<GetDepartmentsQuery, List<DepartmentDto>> GetDepartments { get; }
        public IQueryHandler<DashboardDataQuery, DashboardDataDto> GetDashboardData { get; }
        public IQueryHandler<GetEmployeeByNameQuery, List<EmployeeDto>> GetByName { get; set; }

        public EmployeeQueryHandlers(
            IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>> _GetAll,
            IQueryHandler<GetEmployeeByIdQuery, EmployeeDto> _GetById,
            IQueryHandler<GetEmployeesByDepartmentQuery, List<EmployeeDto>> _GetByDepartment,
            IQueryHandler<GetDepartmentsQuery, List<DepartmentDto>> _GetDepartments,
            IQueryHandler<DashboardDataQuery, DashboardDataDto> getDashboardData,
            IQueryHandler<GetEmployeeByNameQuery, List<EmployeeDto>> _GetByName)
        {
            GetAll = _GetAll;
            GetById = _GetById;
            GetByDepartment = _GetByDepartment;
            GetDepartments = _GetDepartments;
            GetDashboardData = getDashboardData;
            GetByName = _GetByName;
        }
    }
}
