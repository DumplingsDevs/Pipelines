namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskInputGenericResult
{
    public Task<TDispatcherResult> SendAsync<TDispatcherResult>(IInputGenericType<TDispatcherResult> inputType,
        CancellationToken token);
}