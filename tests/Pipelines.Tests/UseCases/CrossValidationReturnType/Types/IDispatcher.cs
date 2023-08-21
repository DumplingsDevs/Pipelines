namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token) where TResult : class;
}