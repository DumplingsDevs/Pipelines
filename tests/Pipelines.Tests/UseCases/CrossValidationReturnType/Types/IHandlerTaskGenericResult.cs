namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskGenericResult<in TInput, TResult> where TInput : IInputType
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}