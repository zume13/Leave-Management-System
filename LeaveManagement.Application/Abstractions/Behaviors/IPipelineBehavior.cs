
namespace LeaveManagement.Application.Abstractions.Behaviors
{
    public interface IPipelineBehavior<Trequest, TResponse>
    {
        Task<TResponse> Handle(Trequest request, TResponse response, Func<Task<TResponse>> next);
    }
}
