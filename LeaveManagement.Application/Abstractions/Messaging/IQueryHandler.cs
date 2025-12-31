

namespace LeaveManagement.Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
    }
}
