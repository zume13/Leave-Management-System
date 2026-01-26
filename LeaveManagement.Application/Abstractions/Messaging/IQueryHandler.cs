using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<ResultT<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
