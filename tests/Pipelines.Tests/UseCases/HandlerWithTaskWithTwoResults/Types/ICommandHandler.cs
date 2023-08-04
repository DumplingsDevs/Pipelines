namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults.Types;

public interface ICommandHandler<in TCommand, TResult, TResult2> where TCommand : ICommand<TResult, TResult2>
    where TResult : class
    where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TCommand command, CancellationToken token);
}