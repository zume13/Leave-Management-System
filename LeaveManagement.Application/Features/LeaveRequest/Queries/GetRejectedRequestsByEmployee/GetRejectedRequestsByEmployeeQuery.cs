using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee
{
    public sealed record GetRejectedRequestsByEmployeeQuery(Guid employeeId, int pageSize, int pageNumber) : IQuery<List<GetRejectedRequestsByEmployeeDto>>;

}
