namespace LeaveManagement.Application.Abstractions.Behaviors
{
    public interface IPipelineBehavior<Trequest, TResponse>
    {
        Task<TResponse> Handle(Trequest request, Func<Task<TResponse>> next, CancellationToken ct = default);
    }
}
