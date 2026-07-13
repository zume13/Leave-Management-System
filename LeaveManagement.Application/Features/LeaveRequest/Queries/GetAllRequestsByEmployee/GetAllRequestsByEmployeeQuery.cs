using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee
{
    public sealed record GetAllRequestsByEmployeeQuery(
        Guid EmployeeId,
        int pageNumber,
        int pageSize
    ) : IQuery<List<GetAllRequestsByEmployeeDto>>;

}
