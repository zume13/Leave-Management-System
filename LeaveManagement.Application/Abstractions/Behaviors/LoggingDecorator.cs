using LeaveManagement.Application.Abstractions.Messaging;
using Serilog;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Abstractions.Behaviors
{
    public class LoggingDecorator
    {

        internal sealed class CommandHandler<TCommand, TResponse>(
            ICommandHandler<TCommand, TResponse> innerHandler,
            ILogger<CommandHandler<TCommand, TResponse>> logger) : ICommandHandler<TCommand, TResponse> where TCommand : ICommand
        {
            public Task<ResultT<TResponse>> Handle(TCommand command, CancellationToken token = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}
