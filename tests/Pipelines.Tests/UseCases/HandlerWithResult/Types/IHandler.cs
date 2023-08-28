namespace Pipelines.Tests.UseCases.HandlerWithResult.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}