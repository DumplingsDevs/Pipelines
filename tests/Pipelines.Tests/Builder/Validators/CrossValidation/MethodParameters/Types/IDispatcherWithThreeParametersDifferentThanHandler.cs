namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IDispatcherWithThreeParametersDifferentThanHandler
{
    public Task<string> SendAsync(IInputType inputType, string text, CancellationToken token);

}