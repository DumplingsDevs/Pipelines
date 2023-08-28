namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerStringResult<in TInput> where TInput : IInputType
{
    public string HandleAsync(TInput command, CancellationToken token);
}