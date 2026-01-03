using SharedKernel.Shared;

namespace LeaveManagement.Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command, CancellationToken token = default);

    }

    public interface ICommandHandler<in TCommand, TReponse> 
        where TCommand : ICommand<TReponse>
    {
        Task<ResultT<TReponse>> Handle(TCommand command, CancellationToken token = default);
    }
}
