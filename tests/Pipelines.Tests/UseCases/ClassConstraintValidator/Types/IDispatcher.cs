namespace Pipelines.Tests.UseCases.ClassConstraintValidator.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}