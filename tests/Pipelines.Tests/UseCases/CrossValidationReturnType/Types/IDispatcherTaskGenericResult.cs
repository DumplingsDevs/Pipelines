namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskGenericResult
{
    public Task<TDispatcherResult> SendAsync<TDispatcherResult>(IInputType inputType, CancellationToken token);
}