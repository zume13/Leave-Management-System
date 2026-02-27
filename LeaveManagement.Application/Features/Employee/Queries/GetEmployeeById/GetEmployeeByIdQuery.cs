using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployee
{
    public sealed record GetEmployeeByIdQuery : IQuery<EmployeeDto>
    {
        [FromRoute(Name = "id")]
        public Guid EmployeeId { get; init; }
        [FromQuery]
        public int pageSize { get; init; }
        [FromQuery]
        public int pageNumber { get; init; }
    }


}
