namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IDispatcherWithThreeParametersDifferentThanHandler
{
    public Task<string> SendAsync(IInputType inputType, string text, CancellationToken token);
}