namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult : struct
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}