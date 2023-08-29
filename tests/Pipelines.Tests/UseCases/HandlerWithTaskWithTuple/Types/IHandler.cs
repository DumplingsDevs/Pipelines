namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

public interface IHandler<in TInput, TResult, TResult2>
    where TInput : IInput<TResult, TResult2>
{
    public Task<(TResult, TResult2)> HandleAsync(TInput input, CancellationToken token);
}