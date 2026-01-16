using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee
{
    public sealed record GetAllRequestByEmployeeQuery(Guid EmployeeId) : IQuery<List<GetAllRequestByEmployeeDto>>;
}
