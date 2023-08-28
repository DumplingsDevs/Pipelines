namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2> where TResult : class where TResult2 : class
{
    public Task<(TResult, TResult2)> HandleAsync(TInput command, CancellationToken token);
}