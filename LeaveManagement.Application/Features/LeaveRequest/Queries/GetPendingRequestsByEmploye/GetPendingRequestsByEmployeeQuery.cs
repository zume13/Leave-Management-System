using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee
{
    public sealed record GetPendingRequestsByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetPendingRequestsByEmployeeDto>>;
}
