namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithResponse;

public interface IDispatcherWithResponse
{
    public Task<TResult> SendAsync<TResult>(IInputWithResponse<TResult> input, CancellationToken token);
}