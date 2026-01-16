using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee
{
    public sealed record GetApprovedRequestsByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetApprovedRequestsByEmployeeDto>>;

}
