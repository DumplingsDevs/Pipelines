namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithInputTypeOnly
{
    public Task<string> SendAsync(IInputType inputType);
}