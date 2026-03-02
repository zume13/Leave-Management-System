using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee
{
    public sealed record GetAllRequestByEmployeeQuery(Guid employeeId, int pageSize, int pageNumber) : IQuery<List<GetAllRequestByEmployeeDto>>;
}
