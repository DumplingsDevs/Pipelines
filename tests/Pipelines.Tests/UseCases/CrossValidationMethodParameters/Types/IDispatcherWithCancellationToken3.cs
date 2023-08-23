namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithCancellationToken3
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}