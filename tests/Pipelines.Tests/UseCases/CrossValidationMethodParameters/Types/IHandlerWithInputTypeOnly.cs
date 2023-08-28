namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithInputTypeOnly<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command);
}