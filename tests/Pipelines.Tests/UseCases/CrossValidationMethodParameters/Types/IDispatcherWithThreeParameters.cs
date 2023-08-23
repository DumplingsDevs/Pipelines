namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithThreeParameters
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}