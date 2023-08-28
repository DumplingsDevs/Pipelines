namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithCancellationToken<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}