namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskStringResult
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}