using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee
{
    public sealed record GetApprovedRequestsByEmployeeQuery(Guid employeeId, int pageSize, int pageNumber) : IQuery<List<GetApprovedRequestsByEmployeeDto>>;
}
