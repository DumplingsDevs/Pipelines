namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithThreeParameters2
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}