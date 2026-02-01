using LeaveManagement.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Decorators
{
    public class LoggingDecorator
    {
        internal sealed class CommandHandler<TCommand, TResponse>(
            ICommandHandler<TCommand, TResponse> innerHandler,
            ILogger<CommandHandler<TCommand, TResponse>> logger
            ) : ICommandHandler<TCommand, TResponse>
            where TCommand : ICommand<TResponse>
        {
            public async Task<ResultT<TResponse>> Handle(TCommand command, CancellationToken token = default)
            {
                var commandName = typeof(TCommand).Name;
                logger.LogInformation("Handling command {commandName}.", commandName);

                var result = await innerHandler.Handle(command, token);

                if (result.isSuccess)
                {
                    logger.LogInformation("Command {commandName} has been executed successfully", commandName);
                }
                else
                {
                    using (LogContext.PushProperty("Error", result.Error, true))
                    {
                        logger.LogError("Completed Command {command} with an error/s", commandName);
                    }
                }

                return result;
            }
        }

        internal sealed class BaseCommandHandler<TCommand>(
            ICommandHandler<TCommand> innerHandler,
            ILogger<BaseCommandHandler<TCommand>> logger)
            : ICommandHandler<TCommand> where TCommand : ICommand
        {
            public async Task<Result> Handle(TCommand command, CancellationToken token = default)
            {
                var commandName = typeof(TCommand).Name;

                logger.LogInformation("Handling command {command}.", commandName);

                var result = await innerHandler.Handle(command, token);

                if (result.isSuccess)
                {
                    logger.LogInformation("Command {command} has been executed successfully", commandName);
                }
                else
                {
                    using(LogContext.PushProperty("Error", result.Error, true))
                    {
                        logger.LogError("Command {comamand} has been executed with error/s", commandName);
                    }
                }

                return result;
            }
        }

        internal sealed class QueryHandler<TQuery, TResponse>(
            IQueryHandler<TQuery, TResponse> innerHandler,
            ILogger<QueryHandler<TQuery, TResponse>> logger)
            : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
        {
            public async Task<ResultT<TResponse>> Handle(TQuery query, CancellationToken token)
            {
                var queryName = typeof(TQuery).Name;

                logger.LogInformation("Handling query {queryName}.", queryName);

                var result = await innerHandler.Handle(query, token);

                if (result.isSuccess)
                {
                    logger.LogInformation("Query {queryName} was executed successfully.", queryName);
                }
                else
                {
                    using (LogContext.PushProperty("Error", result.Error, true))
                    {
                        logger.LogError("Query {queryName} was executed with error/s", queryName);
                    }
                }

                return result;
            }
        }
    }
}
