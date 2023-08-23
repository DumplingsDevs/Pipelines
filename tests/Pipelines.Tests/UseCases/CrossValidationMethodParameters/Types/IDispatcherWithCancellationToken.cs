namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithCancellationToken
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}