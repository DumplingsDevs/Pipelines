namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IDispatcherWithThreeParameters
{
    public Task<string> SendAsync(IInputType inputType, int index, CancellationToken token);
}