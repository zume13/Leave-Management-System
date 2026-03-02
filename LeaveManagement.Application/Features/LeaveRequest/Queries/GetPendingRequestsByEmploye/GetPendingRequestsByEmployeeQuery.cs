using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee
{
    public sealed record GetPendingRequestsByEmployeeQuery(Guid employeeId, int pageSize, int pageNumber) : IQuery<List<GetPendingRequestsByEmployeeDto>>;
}
