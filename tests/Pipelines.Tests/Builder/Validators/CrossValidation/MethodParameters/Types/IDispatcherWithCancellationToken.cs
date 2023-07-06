namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IDispatcherWithCancellationToken
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}