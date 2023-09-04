namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}