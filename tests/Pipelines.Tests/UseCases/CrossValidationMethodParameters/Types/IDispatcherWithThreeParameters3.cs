namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithThreeParameters3
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}