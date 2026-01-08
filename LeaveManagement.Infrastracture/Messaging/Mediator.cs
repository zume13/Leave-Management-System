using LeaveManagement.Application.Abstractions.Behaviors;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;
using Microsoft.Extensions.DependencyInjection;


namespace LeaveManagement.Infrastructure.Messaging
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _sp;
        public Mediator(IServiceProvider sp)
        {
            _sp = sp;
        }
        public Task<ResultT<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
        {
            return PipelineInvoke<ResultT<TResponse>>(query, ct, () =>
            {
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
                dynamic handler = _sp.GetRequiredService(handlerType);
                return handler.Handle((dynamic)query, ct);
            });
        }

        public Task<Result> Send(ICommand command, CancellationToken ct = default)
        {
            return PipelineInvoke<Result>(command, ct, () =>
            {
                var handleType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
                dynamic handler = _sp.GetRequiredService(handleType);
                return handler.Handle((dynamic)command, ct);
            });
        }

        public Task<ResultT<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
        {
            return PipelineInvoke<ResultT<TResponse>>(command, ct, () =>
            {
                var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
                dynamic handler = _sp.GetRequiredService(handlerType);
                return handler.Handle((dynamic)command, ct);
            });
        }

        private async Task<TResponse> PipelineInvoke<TResponse>(
           object request,
           CancellationToken token,
           Func<Task<TResponse>> handler)
        {
            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var behaviors = _sp.GetServices(behaviorType).Cast<dynamic>().Reverse().ToList();

            Func<Task<TResponse>> next = handler;

            foreach (var behavior in behaviors)
            {
                var currentNext = next;
                next = () => behavior.Handle((dynamic)request, token, currentNext);
            }

            return await next();
        }

        //study this
    }
}
