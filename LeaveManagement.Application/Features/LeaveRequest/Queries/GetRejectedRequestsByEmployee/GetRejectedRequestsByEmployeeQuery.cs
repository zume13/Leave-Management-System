using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee
{
    public sealed record GetRejectedRequestsByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetRejectedRequestsByEmployeeDto>>;
}
