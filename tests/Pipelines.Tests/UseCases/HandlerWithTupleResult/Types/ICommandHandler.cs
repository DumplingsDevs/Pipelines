namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface ICommandHandler<in TCommand, TResult, TResult2> where TCommand : ICommand<TResult, TResult2>
    where TResult : class
    where TResult2 : class
{
    public (TResult, TResult2) HandleAsync(TCommand command, CancellationToken token);
}