namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithThreeParameters<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, int index, CancellationToken token);
}