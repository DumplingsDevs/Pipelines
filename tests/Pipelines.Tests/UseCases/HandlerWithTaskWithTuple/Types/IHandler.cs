namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

public interface IHandler<in TCommand, TResult, TResult2>
    where TCommand : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TCommand command, CancellationToken token);
}