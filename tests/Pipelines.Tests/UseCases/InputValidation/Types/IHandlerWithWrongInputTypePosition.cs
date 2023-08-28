namespace Pipelines.Tests.UseCases.InputValidation.Types;

public interface IHandlerWithWrongInputTypePosition<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(CancellationToken token, TInput command);
}