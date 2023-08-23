namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithCancellationToken2
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}