using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Messaging
{
    public interface IMediator
    {
        Task<Result> Send(ICommand command, CancellationToken ct = default);
        Task<ResultT<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
        Task<ResultT<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
    }
}
