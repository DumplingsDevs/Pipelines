namespace Pipelines.Tests.UseCases.NotGenericResult.Types;

public interface IDispatcher
{
    public Task<string> SendAsync(IInput inputWithResult, CancellationToken token);
}