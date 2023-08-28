namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IHandlerWithThreeParameters<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, int index, CancellationToken token);
}