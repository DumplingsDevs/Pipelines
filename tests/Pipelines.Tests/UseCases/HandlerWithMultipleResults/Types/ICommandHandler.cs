namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

public interface ICommandHandler<in TCommand, TResult, TResult2> where TCommand : ICommand<TResult, TResult2>
{
    public (TResult, TResult2) HandleAsync(TCommand command, CancellationToken token);
}