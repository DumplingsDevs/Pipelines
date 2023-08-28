namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IHandlerWithInputTypeOnly<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command);
}