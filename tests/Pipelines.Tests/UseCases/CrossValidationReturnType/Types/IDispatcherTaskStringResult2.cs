namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherTaskStringResult2
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}