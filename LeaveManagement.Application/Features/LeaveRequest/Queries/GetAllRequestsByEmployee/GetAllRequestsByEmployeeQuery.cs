using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee
{
    public sealed record GetAllRequestsByEmployeeQuery(
        Guid EmployeeId,
        int PageNumber,
        int PageSize
    ) : IQuery<List<GetAllRequestsByEmployeeDto>>;

}
