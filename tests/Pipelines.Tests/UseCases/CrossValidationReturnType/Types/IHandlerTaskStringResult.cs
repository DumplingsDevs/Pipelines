namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskStringResult<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}