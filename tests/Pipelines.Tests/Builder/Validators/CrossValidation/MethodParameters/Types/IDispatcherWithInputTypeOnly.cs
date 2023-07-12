namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IDispatcherWithInputTypeOnly
{
    public Task<string> SendAsync(IInputType inputType);
}